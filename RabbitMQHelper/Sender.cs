using Common;
using CQRS.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQHelper
{
    public class Sender<T> : IDisposable where T : class
    {
        private string queueName;
        private string exchangeName;
        private string routingKey;
        private bool durable;
        private bool persistent;

        private IMySerializer serializer;
        private IConnectionFactory connectionFactory;

        private IConnection connection;
        private IModel channel;
        private IBasicProperties properties;


        public Sender(string queueName, IMySerializer serializer, IConnectionFactory connectionFactory, string exchangeName = null, string routingKey = null, bool durable = false, bool persistent = false)
        {
            this.queueName = queueName;
            this.serializer = serializer;
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            this.durable = durable;
            this.persistent = persistent;

            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        IModel Channel
        {
            get
            {
                if (channel == null)
                {
                    connection = connectionFactory.CreateConnection();
                    channel = connection.CreateModel();

                    Dictionary<String, Object> args = new Dictionary<String, Object>();
                    args.Add("x-message-ttl", 3600000L);

                    if (!string.IsNullOrEmpty(exchangeName))
                    {
                        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable, false, args);
                    }

                    channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: args);

                    properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = (Byte)(persistent ? 2 : 1);
                    properties.Expiration = "36000000";
                    properties.ContentType = "text/plain";

                    if (!string.IsNullOrEmpty(exchangeName) && !string.IsNullOrEmpty(routingKey))
                    {
                        channel.QueueBind(queueName, exchangeName, routingKey, args);
                    }

                    channel.BasicReturn += (s, a) =>
                    {
                        Console.WriteLine(a);
                    };
                }

                return channel;
            }
        }


        public void Send(T t)
        {
            string message = serializer.Serializ(t);
            var body = Encoding.UTF8.GetBytes(message);

            Channel.BasicPublish(exchangeName, routingKey, true, properties, body);
        }

        public void Dispose()
        {
            if (channel != null)
            {
                channel.RemoveAllEventHandlers();
                channel.Close();
            }

            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
