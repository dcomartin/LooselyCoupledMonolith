using Billing;
using Billing.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sales;
using Sales.Contracts;
using Shipping;
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
                    routing.MapBilling();
                    routing.MapSales();
                    routing.MapShipping();

                    endpointConfiguration.SendOnly();

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
