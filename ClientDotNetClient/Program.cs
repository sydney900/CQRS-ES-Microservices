using CQRS.Domain;
using KafkaHelper;
using System;
using System.Collections.Generic;

namespace ClientProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "localhost:9092" }
            };

            var topic = "Clients-Topic:1:1";

            try
            {
                using (KafkaCommandSender publisher = new KafkaCommandSender(config, new MySerializer(), topic))
                {
                    IEnumerable<string> names = NameGeneratorHelper.Generate(10);
                    foreach (string name in names)
                    {
                        publisher.Send(new CreateClientCommand(Guid.NewGuid(), name));

                        System.Threading.Tasks.Task.Delay(30000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            Console.WriteLine("10 people name published!");
        }
    }
}
