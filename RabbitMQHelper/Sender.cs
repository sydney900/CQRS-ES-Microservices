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
                    var connection = factory.CreateConnection();
                    channel = connection.CreateModel();
                    channel.QueueDeclare(queueName, durable, false, false, null);
                    properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = (Byte)(persistent ? 2 : 1);
                }

                return channel;
            }
        }

        public Sender(string hostName, string queueName, IMySerializer serializer, bool durable = false, bool persistent = false)
        {
            this.hostName = hostName;
            this.queueName = queueName;
            this.serializer = serializer;
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
