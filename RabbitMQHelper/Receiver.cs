using Common;
using CQRS.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper
{

    public class Receiver<T> : IDisposable where T : class
    {
        private string hostName;
        private string queueName;
        private IMySerializer serializer;
        IReceivedHandler<T> receivedHandler;
        private bool durable;
        private bool persistent;

        public Receiver(string hostName, string queueName, IMySerializer serializer, IReceivedHandler<T> receivedHandler, bool durable = false, bool persistent = false)
        {
            this.hostName = hostName;
            this.queueName = queueName;
            this.serializer = serializer;
            this.receivedHandler = receivedHandler;
            this.durable = durable;
            this.persistent = persistent;
        }

        public void Receive(T t)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    //Received message
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

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
