namespace APIGateway
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
//Install-Package Ocelot
//https://carlos.mendible.com/2017/05/08/step-by-step-kafka-pub-sub-with-docker-and-net-core/
//https://github.com/cmendible/dotnetcore.samples/blob/master/kafka.pubsub.console/docker/docker-compose.yml
//https://koukia.ca/a-microservices-implementation-journey-part-6-9b818e491336
//https://github.com/dotnet-architecture/eShopOnContainers
//https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/integration-event-based-microservice-communications
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseUrls("http://localhost:9000");

            var host = builder.Build();
            host.Run();
        }
    }
}

