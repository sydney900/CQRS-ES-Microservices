using Common;
using CQRS.Core;
using CQRS.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace RabbitMQHelper.Test
{
    [TestClass]
    public class SenderUnitTest
    {
        Mock<IMySerializer> mockMySerializer;
        Mock<IConnectionFactory> mockConnectionFactory;
        Mock<IBasicProperties> mockIBasicProperties;
        Mock<IModel> mockChannel;
        Mock<IConnection> mockConnection;


        Client client;
        string sClient;

        Sender<Client> sender;

        [TestInitialize]
        public void Setup()
        {
            mockMySerializer = new Mock<IMySerializer>();
            mockConnectionFactory = new Mock<IConnectionFactory>();
            mockIBasicProperties = new Mock<IBasicProperties>();
            mockChannel = new Mock<IModel>();
            mockConnection = new Mock<IConnection>();


            var id = new Guid();
            client = new Client(id, "me");
            sClient = JsonConvert.SerializeObject(client);

            mockMySerializer.Setup(m => m.Serializ(It.IsAny<Client>())).Returns(sClient).Verifiable();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullSerializerParameter_ShouldRaiseException()
        {
            sender = new Sender<Client>("", null, mockConnectionFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullConnectionFactoryParameter_ShouldRaiseException()
        {
            sender = new Sender<Client>("", mockMySerializer.Object, null);
        }

        private void PrepareMock()
        {
            mockChannel.Setup(m => m.CreateBasicProperties()).Returns(mockIBasicProperties.Object).Verifiable();
            mockChannel.Setup(m => m.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>())).Verifiable();
            mockChannel.Setup(m => m.QueueBind(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())).Verifiable();

            mockConnection.Setup(m => m.CreateModel()).Returns(mockChannel.Object).Verifiable();
            mockConnectionFactory.Setup(m => m.CreateConnection()).Returns(mockConnection.Object).Verifiable();

            sender = new Sender<Client>("", mockMySerializer.Object, mockConnectionFactory.Object);
        }

        [TestMethod]
        public void Send_ShouldCall_Serializ_In_IMySerializer()
        {
            PrepareMock();
            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

            sender.Send(client);

            mockMySerializer.Verify(m => m.Serializ(It.IsAny<Client>()), Times.Once);
        }

        [TestMethod]
        public void Send_ShouldCall_CreateConnection()
        {
            PrepareMock();
            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));
            
            sender.Send(client);

            mockConnectionFactory.Verify(m => m.CreateConnection(), Times.Once);
        }

        [TestMethod]
        public void Send_ShouldCall_CreateModel()
        {
            PrepareMock();
            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));            

            sender.Send(client);

            mockConnection.Verify(m => m.CreateModel(), Times.Once);
        }

        [TestMethod]
        public void Send_WithoutExchangeName_ShouldNotCall_ExchangeDeclare()
        {
            PrepareMock();
            sender = new Sender<Client>("", mockMySerializer.Object, mockConnectionFactory.Object);
            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

            sender.Send(client);

            mockChannel.Verify(m => m.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()), Times.Never);
        }

        [TestMethod]
        public void Send_WithExchangeName_ShouldCall_ExchangeDeclare()
        {
            var exChangeName = "MyExchange";

            PrepareMock();
            sender = new Sender<Client>("", mockMySerializer.Object, mockConnectionFactory.Object, exChangeName);
            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

            sender.Send(client);
            mockChannel.Verify(m => m.ExchangeDeclare(It.Is<string>(a => a == exChangeName), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
        }

        [TestMethod]
        public void Send_WithExchangeNameAndRoutingKay_ShouldCall_QueueBindWithCorrectParameter()
        {
            mockIBasicProperties.SetupSet(m => m.DeliveryMode = 2).Verifiable();

            PrepareMock();

            sender = new Sender<Client>("MyQueue", mockMySerializer.Object, mockConnectionFactory.Object, "MyExchange", "MyRoutingKey", true, true);

            mockChannel.Setup(m => m.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));


            sender.Send(client);

            mockIBasicProperties.Verify();
            mockChannel.Verify(m => m.QueueBind("MyQueue", "MyExchange", "MyRoutingKey", It.Is<IDictionary<string, object>>(d => Convert.ToInt64(d["x-message-ttl"]) == 3600000L)), Times.Once);
        }

        [TestMethod]
        public void Disppse_WithoutAnySend_ShouldWork()
        {
            sender = new Sender<Client>("MyQueue", mockMySerializer.Object, mockConnectionFactory.Object, "MyExchange", "MyRoutingKey", true, true);
            sender.Dispose();
        }

        [TestMethod]
        public void Disppse_WithSend_ShouldWork()
        {
            PrepareMock();
            sender = new Sender<Client>("MyQueue", mockMySerializer.Object, mockConnectionFactory.Object, "MyExchange", "MyRoutingKey", true, true);
            sender.Send(client);
            sender.Dispose();
        }

        [TestMethod]
        public void Disppse_WithSend_ShouldCallClose()
        {
            PrepareMock();
            mockChannel.Setup(m => m.Close()).Verifiable();
            mockConnection.Setup(m => m.Close()).Verifiable();

            sender = new Sender<Client>("MyQueue", mockMySerializer.Object, mockConnectionFactory.Object, "MyExchange", "MyRoutingKey", true, true);
            sender.Send(client);
            sender.Dispose();

            mockChannel.Verify(m => m.Close(), Times.Once);
            mockConnection.Verify(m => m.Close(), Times.Once);
        }

        [TestMethod]
        public void Disppse_WithoutSend_ShouldNotCallClose()
        {
            PrepareMock();
            mockChannel.Setup(m => m.Close()).Verifiable();
            mockConnection.Setup(m => m.Close()).Verifiable();

            sender = new Sender<Client>("MyQueue", mockMySerializer.Object, mockConnectionFactory.Object, "MyExchange", "MyRoutingKey", true, true);
            sender.Dispose();

            mockChannel.Verify(m => m.Close(), Times.Never);
            mockConnection.Verify(m => m.Close(), Times.Never);
        }
    }
}
