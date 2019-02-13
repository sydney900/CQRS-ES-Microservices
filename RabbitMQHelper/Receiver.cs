using Common;
using CQRS.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace RabbitMQHelper
{

    public class Receiver<T> : IDisposable, IRabbitMqReceiver where T : class
    {
        private string queueName;
        private string exchangeName;
        private string routingKey;
        private bool durable;
        private bool persistent;

        private IMySerializer serializer;
        private IReceivedHandler<T> receivedHandler;
        private IConnectionFactory connectionFactory;
        private IEventingBasicConsumerFactory eventingBasicConsumerFactory;

        private IConnection connection;
        private IModel channel;
        private EventingBasicConsumer consumer;
        private string consumerTag;

        public Receiver(string queueName, IMySerializer serializer, IReceivedHandler<T> receivedHandler, IConnectionFactory connectionFactory, IEventingBasicConsumerFactory eventingBasicConsumerFactory, string exchangeName = null, string routingKey = null, bool durable = false, bool persistent = false)
        {
            this.queueName = queueName;
            this.exchangeName = exchangeName;
            this.routingKey = routingKey;
            this.durable = durable;
            this.persistent = persistent;

            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.receivedHandler = receivedHandler ?? throw new ArgumentNullException(nameof(receivedHandler));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.eventingBasicConsumerFactory = eventingBasicConsumerFactory ?? throw new ArgumentNullException(nameof(eventingBasicConsumerFactory));
        }

        public void Connect()
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

            if (!string.IsNullOrEmpty(exchangeName))
            {
                //channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable);
            }

            Dictionary<String, Object> args = new Dictionary<String, Object>();
            args.Add("x-message-ttl", 3600000L);
            var queueDeclareOk = channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: args);
            //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            if (!string.IsNullOrEmpty(exchangeName) && !string.IsNullOrEmpty(routingKey))
            {
                channel.QueueBind(queueName, exchangeName, routingKey, args);
            }

            var consumer = eventingBasicConsumerFactory.Create(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                        //Received message
                        var message = ea.Body.ToUTF8String();

                    T r = serializer.DeserializeObject<T>(message);
                    receivedHandler.Process(r);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message, ex);
                }
            };
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine(queueDeclareOk?.MessageCount);
            Console.WriteLine(queueDeclareOk?.ConsumerCount);

        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(consumerTag) && channel != null)
            {
                channel.BasicCancel(consumerTag);
            }

            if (consumer != null)
                consumer.RemoveAllEventHandlers();

            if (channel != null)
                channel.Dispose();

            if (connection != null)
                connection.Dispose();

            this.RemoveAllEventHandlers();
        }
    }
}
