using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQHelper
{
    public interface IEventingBasicConsumerFactory
    {
        EventingBasicConsumer Create(IModel chanel);
    }
}
