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
    public class RPCClient<Req, Res>: IDisposable
        where Req : class
        where Res : class
    {
        private string hostName;
        private string rpcQueueName;
        private IMySerializer serializer;

        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RPCClient(string hostName, string rpcQueueName, IMySerializer serializer)
        {
            this.hostName = hostName;
            this.rpcQueueName = rpcQueueName;
            this.serializer = serializer;

            Setup();
        }

        private void Setup()
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare();
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(replyQueueName, true, consumer);
        }

        public Res Query(Req req)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            string message = serializer.Serializ(req);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", rpcQueueName, props, messageBytes);

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    message = Encoding.UTF8.GetString(ea.Body);
                    return serializer.DeserializeObject<Res>(message);
                }
            }
        }


        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}
