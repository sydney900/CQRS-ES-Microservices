using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQHelper
{
    public class EventingBasicConsumerFactory : IEventingBasicConsumerFactory
    {
        public EventingBasicConsumer Create(IModel channel)
        {
            return new EventingBasicConsumer(channel);
        }
    }
}
