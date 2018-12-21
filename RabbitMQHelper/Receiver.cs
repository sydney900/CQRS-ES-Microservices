using Common;
using CQRS.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace RabbitMQHelper
{

    public class Receiver<T> : IDisposable where T : class
    {
        private string hostName;
        private string queueName;
        private int port;
        private string userName;
        private string password;
        private string vhost;
        private string exchangeName;
        private string routingKey;
        private bool durable;
        private bool persistent;

        private IMySerializer serializer;
        IReceivedHandler<T> receivedHandler;

        public Receiver(string hostName, string queueName, IMySerializer serializer, IReceivedHandler<T> receivedHandler, int port = 5672, string userName = null, string password = null, string vhost = null, string exchangeName = null, string routingKey = null, bool durable = false, bool persistent = false)
        {
            this.hostName = hostName;
            this.queueName = queueName;
            this.port = port;
            this.userName = userName;
            this.password = password;
            this.vhost = vhost;
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            this.durable = durable;
            this.persistent = persistent;

            this.serializer = serializer;
            this.receivedHandler = receivedHandler;
        }

        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                factory.UserName = userName;
                factory.Password = password;
            }
            if (!string.IsNullOrEmpty(vhost))
            {
                factory.VirtualHost = vhost;
            }
            factory.Port = port;

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                if (!string.IsNullOrEmpty(exchangeName))
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                }

                Dictionary<String, Object> args = new Dictionary<String, Object>();
                args.Add("x-message-ttl", 3600000L);
                channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: args);
                //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                if (!string.IsNullOrEmpty(exchangeName) && !string.IsNullOrEmpty(routingKey))
                {
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    //Received message
                    var message = ea.Body.ToUTF8String();

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                    T r = serializer.DeserializeObject<T>(message);
                    receivedHandler.Process(r);
                };
                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }
        }

        public void Dispose()
        {
            this.RemoveAllEventHandlers();
        }
    }
}
