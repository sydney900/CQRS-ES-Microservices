using System;
using System.Text;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Common;
using CQRS.Core;

namespace KafkaHelper
{
    public class KafkaSubscriber<T> : IDisposable where T: class
    {
        private Dictionary<string, object> config;
        private Consumer<Null, string> consumer;
        private IMySerializer serializer;
        IReceivedHandler<T> receivedHandler;
        private string topic;
        private int millisecondsTimeout;
        private bool disposing;

        public KafkaSubscriber(Dictionary<string, object> config, string topic, IMySerializer serializer, IReceivedHandler<T> receivedHandler, int millisecondsTimeout = 100)
        {
            this.config = config;
            this.topic = topic;
            this.serializer = serializer;
            this.receivedHandler = receivedHandler;
            this.millisecondsTimeout = millisecondsTimeout;
        }

        private Consumer<Null, string> Consumer
        {
            get
            {
                if (!disposing && consumer == null)
                {
                    consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8));

                    consumer.OnMessage += OnMessageHandler;
                    consumer.OnError += OnErrorHandler;
                    consumer.OnConsumeError += OnConsumeErrorHandler;                    
                }

                return consumer;
            }
        }

        private void OnMessageHandler(object sender, Message<Null, string> msg)
        {
            Console.WriteLine("Read '{0}' from: {1}", msg.Value, msg.TopicPartitionOffset);

            T r = serializer.DeserializeObject<T>(msg.Value);
            receivedHandler.Process(r);

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
            Consumer.Subscribe(topic);

            while (true)
            {
                Consumer.Poll(TimeSpan.FromMilliseconds(millisecondsTimeout));
            }
        }

        public void Dispose()
        {
            disposing = true;

            if (consumer != null)
            {
                consumer.Unsubscribe();
                consumer.Unassign();
            }

            this.GetType().RemoveAllEventHandlers();
            if (consumer != null)
            {
                consumer.Dispose();
                consumer = null;
            }
        }
    }
}