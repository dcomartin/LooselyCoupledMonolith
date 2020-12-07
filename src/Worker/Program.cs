using Billing;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sales;
using Shipping;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
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


                    return endpointConfiguration;
                })
                .ConfigureServices(services =>
                {
                    services.AddSales();
                    services.AddShipping();
                });
    }
}
