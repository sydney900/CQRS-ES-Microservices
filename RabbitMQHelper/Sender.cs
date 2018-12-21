using CQRS.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper
{
    public class Sender<T> : IDisposable where T : class
    {
        private string hostName;
        private string queueName;
        private IMySerializer serializer;
        private int port;
        private string userName;
        private string password;
        private string vhost;
        private string exchangeName;
        private string routingKey;
        private bool durable;
        private bool persistent;

        private IConnection connection;
        private IModel channel;
        private IBasicProperties properties;

        IModel Channel
        {
            get
            {
                if (channel == null)
                {
                    var factory = new ConnectionFactory() { HostName = hostName };
                    connection = factory.CreateConnection();
                    channel = connection.CreateModel();

                    if (!string.IsNullOrEmpty(exchangeName))
                    {
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                    }

                    Dictionary<String, Object> args = new Dictionary<String, Object>();
                    args.Add("x-message-ttl", 3600000L);
                    channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: args);
                    properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = (Byte)(persistent ? 2 : 1);

                    if (!string.IsNullOrEmpty(exchangeName) && !string.IsNullOrEmpty(routingKey))
                    {
                        channel.QueueBind(queueName, exchangeName, routingKey, null);
                    }
                }

                return channel;
            }
        }

        public Sender(string hostName, string queueName, IMySerializer serializer, int port = 5672, string userName = null, string password = null, string vhost = null, string exchangeName = null, string routingKey = null, bool durable = false, bool persistent = false)
        {
            this.hostName = hostName;
            this.queueName = queueName;
            this.serializer = serializer;
            this.port = port;
            this.userName = userName;
            this.password = password;
            this.vhost = vhost;
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            this.durable = durable;
            this.persistent = persistent;
        }

        public void Send(T t)
        {
            string message = serializer.Serializ(t);
            var body = Encoding.UTF8.GetBytes(message);

            Channel.BasicPublish("", queueName, properties, body);
        }

        public void Dispose()
        {
            if (channel != null)
            {
                channel.Close();
                connection.Close();
            }
        }
    }
}
