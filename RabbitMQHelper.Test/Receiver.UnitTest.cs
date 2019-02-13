using Common;
using CQRS.Core;
using CQRS.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace RabbitMQHelper.Test
{
    [TestClass]
    public class ReceiverUnitTest
    {
        Mock<IMySerializer> mockMySerializer;
        Mock<IReceivedHandler<Client>> mockReceiveHandler;
        Mock<IConnectionFactory> mockConnectionFactory;
        Mock<IEventingBasicConsumerFactory> mockEventingBasicConsumerFactory;
        Client client;

        [TestInitialize]
        public void Setup()
        {
            mockMySerializer = new Mock<IMySerializer>();
            mockReceiveHandler = new Mock<IReceivedHandler<Client>>();
            mockConnectionFactory = new Mock<IConnectionFactory>();
            mockEventingBasicConsumerFactory = new Mock<IEventingBasicConsumerFactory>();

            var id = new Guid();
            client = new Client(id, "me");

            mockMySerializer.Setup(m => m.DeserializeObject<Client>(It.IsAny<string>())).Returns(client).Verifiable();
            mockReceiveHandler.Setup(m => m.Process(It.IsAny<Client>())).Verifiable();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullSerializerParameter_ShouldRaiseException()
        {
            var service = new Receiver<Client>("", null, mockReceiveHandler.Object, mockConnectionFactory.Object, mockEventingBasicConsumerFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullReceivedHandlerParameter_ShouldRaiseException()
        {
            var service = new Receiver<Client>("", mockMySerializer.Object, null, mockConnectionFactory.Object, mockEventingBasicConsumerFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullConnectionFactoryParameter_ShouldRaiseException()
        {
            var service = new Receiver<Client>("", mockMySerializer.Object, mockReceiveHandler.Object, null, mockEventingBasicConsumerFactory.Object);
        }

        [TestMethod]
        public void Receive_WhenGetObject_ShouldBeProcessed()
        {
            //            BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments, IBasicConsumer consumer);
            var mockChannel = new Mock<IModel>();
            mockChannel.Setup(m => m.IsOpen).Returns(true);

            EventingBasicConsumer consumer = new EventingBasicConsumer(mockChannel.Object);
            mockChannel.Setup(m => m.BasicConsume(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), null, It.IsAny<IBasicConsumer>()))
                .Returns("test")
                .Callback(() =>
                {
                    string nsClient = (new MySerializer()).Serializ(client);
                    byte[] body = Encoding.UTF8.GetBytes(nsClient);

                    consumer.HandleBasicDeliver("", 0, false, "", "", null, body);

                    mockMySerializer.Verify(m => m.DeserializeObject<Client>(nsClient), Times.Once);
                    mockReceiveHandler.Verify(m => m.Process(client), Times.Once);
                });


            mockEventingBasicConsumerFactory.Setup(m => m.Create(It.IsAny<IModel>())).Returns(consumer);

            var mockConnection = new Mock<IConnection>();
            mockConnection.Setup(m => m.IsOpen).Returns(true);
            mockConnection.Setup(m => m.CreateModel()).Returns(mockChannel.Object);

            mockConnectionFactory.Setup(m => m.CreateConnection()).Returns(mockConnection.Object);

            var service = new Receiver<Client>("", mockMySerializer.Object, mockReceiveHandler.Object, mockConnectionFactory.Object, mockEventingBasicConsumerFactory.Object);

            service.Connect();
            service.Dispose();
        }

        Receiver<Client> receiver;
        Mock<IModel> mockChannel;
        Mock<IConnection> mockConnection;
        private void PrepareMock()
        {
            mockChannel = new Mock<IModel>();
            mockChannel.SetupAllProperties();

            mockConnection = new Mock<IConnection>();
            mockConnection.Setup(m => m.IsOpen).Returns(true);
            mockConnection.Setup(m => m.CreateModel()).Returns(mockChannel.Object);

            mockConnectionFactory.Setup(m => m.CreateConnection()).Returns(mockConnection.Object);

            receiver = new Receiver<Client>("", mockMySerializer.Object, mockReceiveHandler.Object, mockConnectionFactory.Object, mockEventingBasicConsumerFactory.Object);
        }

        [TestMethod]
        public void Disppse_WithoutRecieiveCalled_ShouldWork()
        {
            PrepareMock();

            receiver.Dispose();
        }

        [TestMethod]
        public void Dispose_WithRecieiveCalled_ShouldWork()
        {
            PrepareMock();

            EventingBasicConsumer consumer = new EventingBasicConsumer(mockChannel.Object);
            mockEventingBasicConsumerFactory.Setup(m => m.Create(It.IsAny<IModel>())).Returns(consumer); 
            mockChannel.Setup(m => m.BasicConsume(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), null, It.IsAny<IBasicConsumer>()));

            var receiver = new Receiver<Client>("", mockMySerializer.Object, mockReceiveHandler.Object, mockConnectionFactory.Object, mockEventingBasicConsumerFactory.Object);
            receiver.Connect();
            receiver.Dispose();
        }

    }
}
