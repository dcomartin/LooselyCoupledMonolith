using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureServices(services =>
                {
                    services.AddSales();
                    services.AddShipping();

                    services.AddCap(options =>
                    {
                        options.UseSqlServer("Server=localhost\\SQLExpress;Database=Demo;Trusted_Connection=Yes;");
                        options.UseRabbitMQ("localhost");
                    });

                });
    }
}
