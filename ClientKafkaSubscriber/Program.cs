using Common;
using CQRS.Core;
using CQRS.Domain;
using KafkaHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ClientKafkaSubscriber
{
    class Program
    {
        static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();


//            var config = Configuration.GetValue<Dictionary<string, object>>("KafkaSetting:Config");
            var config = Configuration.GetSection("KafkaSetting:Config").GetChildren()
                    .Select(item => new KeyValuePair<string, object>(item.Key, item.Value))
                    .ToDictionary(x => x.Key, x => x.Value);
            var topic = Configuration["KafkaSetting:Topic"];
            var millisecondsTimeout = Configuration.GetValue<int>("KafkaSetting:MillisecondsTimeout");

            try
            {
                using (var publisher = new KafkaSubscriber<CreateClientCommand>(config, topic, new MySerializer(), new TestHandler(), millisecondsTimeout))
                {
                    publisher.Subscriber();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }

    class TestHandler : IReceivedHandler<CreateClientCommand>
    {
        public void Process(CreateClientCommand t)
        {
            Console.WriteLine(t.Name);
        }
    }
}
