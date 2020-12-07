using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sales.Contracts;
using Shipping.Contracts;

namespace AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("LooselyCoupledMonolith");
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

                    var routing = transport.Routing();

                    routing.RouteToEndpoint(
                        assembly: typeof(ShippingLabelCreated).Assembly,
                        destination: "LooselyCoupledMonolith");

                    routing.RouteToEndpoint(
                        assembly: typeof(OrderPlaced).Assembly,
                        destination: "LooselyCoupledMonolith");

                    //endpointConfiguration.SendOnly();

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
