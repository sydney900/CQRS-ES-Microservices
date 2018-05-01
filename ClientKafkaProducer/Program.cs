using CQRS.Core;
using CQRS.Domain;
using KafkaHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClientKafkaProducer
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


            var config = Configuration.GetValue<Dictionary<string, object>>("KafkaSetting:Config");
            var topic = Configuration["KafkaSetting:Topic"];

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
