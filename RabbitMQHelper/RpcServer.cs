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

    public class RpcServer<Req, Res>
        where Req : class
        where Res : class
    {
        private string hostName;
        private string rpcQueueName;
        private IMySerializer serializer;
        private IQueryHanlder<Req, Res> queryHandler;
        private ILog log;
        private bool stop;

        public RpcServer(string hostName, string rpcQueueName, IMySerializer serializer, IQueryHanlder<Req, Res> queryHandler, ILog log)
        {
            this.hostName = hostName;
            this.rpcQueueName = rpcQueueName;
            this.serializer = serializer;
            this.queryHandler = queryHandler;
            this.log = log;
        }

        public Task Run()
        {
            this.stop = false;

            return Task.Run(() =>
            {
                var factory = new ConnectionFactory() { HostName = hostName };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(rpcQueueName, false, false, false, null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new QueueingBasicConsumer(channel);
                        channel.BasicConsume(rpcQueueName, false, consumer);

                        //then waiting RPC requests
                        while (true)
                        {
                            if (stop) break;

                            string response = null;
                            var ea =
                                (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                            //got request
                            var body = ea.Body;
                            var props = ea.BasicProperties;
                            var replyProps = channel.CreateBasicProperties();
                            replyProps.CorrelationId = props.CorrelationId;

                            try
                            {
                                //processing request
                                var message = Encoding.UTF8.GetString(body);
                                var req = serializer.DeserializeObject<Req>(message);
                                var res = queryHandler.ExecuteQuery(req);
                                response = serializer.Serializ(res);
                            }
                            catch (Exception e)
                            {
                                if (log != null)
                                    log.LogError(e.ToString());

                                response = "";
                            }
                            finally
                            {
                                //send back to the client
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                        }
                    }
                }
            });            
        }

        public void Stop()
        {
            this.stop = true;
        }
    }
}
