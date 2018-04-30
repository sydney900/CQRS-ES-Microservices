using System;
using System.Text;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CQRS.Common
{
    public class KafkaPublisher
    {
        private Dictionary<string, object> config;
        private Producer<Null, string> producer;
        private IMySerializer serializer;
        private int millisecondsTimeout;

        public KafkaPublisher(Dictionary<string, object> config, IMySerializer serializer, int millisecondsTimeout = 100)
        {
            this.config = config;
            this.serializer = serializer;
            this.millisecondsTimeout = millisecondsTimeout;
        }

        private Producer<Null, string> Producer
        {
            get
            {
                if (producer == null)
                {
                    producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8));
                }

                return producer;
            }
        }

        public async Task<Message<Null, string>> Publish<T>(T t, string topic)
        {
            return await Producer.ProduceAsync(topic, null, serializer.Serializ(t));
        }

        public void Stop()
        {
            if (producer!=null)
            {
                producer.Flush(millisecondsTimeout);
            }
        }
    }

    public class KafkaSubscriber
    {
        private Dictionary<string, object> config;
        private Consumer<Null, string> consumer;
        private IMySerializer serializer;
        private string topic;
        private int millisecondsTimeout;

        public KafkaSubscriber(Dictionary<string, object> config, IMySerializer serializer, string topic, int millisecondsTimeout = 100)
        {
            this.config = config;
            this.serializer = serializer;
            this.topic = topic;
            this.millisecondsTimeout = millisecondsTimeout;
        }

        private Consumer<Null, string> Consumer
        {
            get
            {
                if (consumer == null)
                {
                    consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8));

                    consumer.OnMessage += OnMessageHandler;
                    consumer.OnError += OnErrorHandler;
                    consumer.OnConsumeError += OnConsumeErrorHandler;

                    consumer.Subscribe(topic);
                }

                return consumer;
            }
        }

        private void OnMessageHandler(object sender, Message<Null, string> msg)
        {
            Console.WriteLine("Read '{0}' from: {1}", msg.Value, msg.TopicPartitionOffset);
            consumer.CommitAsync(msg);
        }

        private void OnErrorHandler(object sender, Error error)
        {
            Console.WriteLine("Error: {0}", error);
        }

        private void OnConsumeErrorHandler(object sender, Message msg)
        {
            Console.WriteLine("Consume error ({0}): {1}", msg.TopicPartitionOffset, msg.Error);
        }

        public void Subscriber()
        {
            while (true)
            {
                consumer.Poll(TimeSpan.FromMilliseconds(millisecondsTimeout));
            }
        }

        public void Stop()
        {
            if (consumer!=null)
            {
                consumer.Unsubscribe();
                consumer.Unassign();
            }

            this.GetType().RemoveAllEventHandlers();
        }

    }

    public interface IMySerializer
    {
        string Serializ(object value);
        T DeserializeObject<T>(string value);
    }

    public class MySerializer : IMySerializer
    {
        public string Serializ(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}