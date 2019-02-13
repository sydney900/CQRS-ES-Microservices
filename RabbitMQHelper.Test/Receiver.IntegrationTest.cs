using System;
using System.IO;
using CQRS.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;

namespace RabbitMQHelper.Test
{
    [TestCategory("LocalTest")]
    [TestClass]
    public class ReceivertIntegrationTest
    {
        private string nsTrip;
        private NetstarTrip trip;

        IMySerializer serializer;
        IConnectionFactory connectionFactory;
        TripReceivedHandler receiverHandler;
        EventingBasicConsumerFactory eventingBasicConsumerFactory;

        string queueName = "ez2c.locations";
        string exchangeName = "data";
        string routingKey = "locations.#";
        bool durable = true;
        bool persistent = false;

        string hostName = "Pin-LT06";
        //string hostName = "PINDT-BINGW";
        int port=5672;
        string userName= "ez2c";
        string password= "tjeFZE455sPMMMYT";
        string vhost= "ez2c";
        int timeOutSeconds= 180;


        [TestInitialize]
        public void Setup()
        {
            serializer = new MySerializer();
            connectionFactory = (new MyConnectFactory(hostName, port, userName, password, vhost, timeOutSeconds)).CreateConnectFactory();
            receiverHandler = new TripReceivedHandler();
            eventingBasicConsumerFactory = new EventingBasicConsumerFactory();
        }


        [TestMethod]
        public void Receiver_ShouldWork()
        {
            var receiver = new Receiver<NetstarTrip>(queueName, serializer, receiverHandler, connectionFactory, eventingBasicConsumerFactory, exchangeName, routingKey, durable, persistent);
            receiver.Connect();
            //receiver.Dispose();
        }
    }
}
