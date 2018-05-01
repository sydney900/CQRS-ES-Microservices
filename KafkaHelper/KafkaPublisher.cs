using System;
using System.Text;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Threading.Tasks;
using CQRS.Core;

namespace KafkaHelper
{
    public class KafkaCommandSender : IDisposable, ICommandSender
    {
        private Dictionary<string, object> config;
        private Producer<Null, string> producer;
        private IMySerializer serializer;
        private readonly string topic;
        private int millisecondsTimeout;
        private bool disposing;

        public KafkaCommandSender(Dictionary<string, object> config, IMySerializer serializer, string topic, int millisecondsTimeout = 100)
        {
            this.config = config;
            this.serializer = serializer;
            this.topic = topic;
            this.millisecondsTimeout = millisecondsTimeout;
        }

        private Producer<Null, string> Producer
        {
            get
            {
                if (!disposing && producer == null)
                {
                    producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
                }

                return producer;
            }
        }

        public void Dispose()
        {
            disposing = true;

            if (producer != null)
            {
                producer.Flush(millisecondsTimeout);
                producer.Dispose();
                producer = null;
            }
        }

        public async Task<Message<Null, string>> PublishAsync<T>(T t)
        {
            return await Producer.ProduceAsync(topic, null, serializer.Serializ(t));
        }

        public void Send(ICommand command)
        {
            var result = Producer.ProduceAsync(topic, null, serializer.Serializ(command)).Result;
        }
    }
}