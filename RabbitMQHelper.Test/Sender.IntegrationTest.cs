using System;
using System.IO;
using System.Threading.Tasks;
using CQRS.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;

namespace RabbitMQHelper.Test
{
    [TestCategory("LocalTest")]
    [TestClass]
    public class SenderIntegrationTest
    {
        private string nsTrip;
        private NetstarTrip trip;

        IMySerializer serializer;
        IConnectionFactory connectionFactory;

        string queueName = "ez2c.locations";
        string exchangeName = "data";
        string routingKey = "locations.#";
        bool durable = true;
        bool persistent = false;

        string hostName = "Pin-LT06";
        //string hostName = "PINDT-BINGW";
        int port = 5672;
        string userName = "ez2c";
        string password = "tjeFZE455sPMMMYT";
        string vhost = "ez2c";
        int timeOutSeconds = 180;


        [TestInitialize]
        public void Setup()
        {
            serializer = new MySerializer();

            nsTrip = File.ReadAllText("NetstarTrip.txt");
            trip = serializer.DeserializeObject<NetstarTrip>(nsTrip);
            trip.Imei = 987654321; //This is the UnitCode of a vehicle

            connectionFactory = (new MyConnectFactory(hostName, port, userName, password, vhost, timeOutSeconds)).CreateConnectFactory();
        }


        [DataTestMethod]
        [DataRow("7ccdde51-f429-4e81-a514-671232bba3ab")]
        [DataRow("46E9DA44-74F7-412A-96D4-F28641C9415D")]
        [DataRow("1663FC23-06F3-4E8B-BAE4-C038ED604E9D")]
        public async Task Sender_ShouldWork(string tripId)
        {
            try
            {
                trip.Id = tripId;

                var sender = new Sender<NetstarTrip>(queueName, serializer, connectionFactory, exchangeName, routingKey, durable, persistent);
                sender.Send(trip);
                //sender.Dispose();

            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }
    }
}
