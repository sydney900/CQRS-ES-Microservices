using RabbitMQ.Client;

namespace RabbitMQHelper
{
    public interface IMyConnectFactory
    {
        ConnectionFactory CreateConnectFactory();
    }
}