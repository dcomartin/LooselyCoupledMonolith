using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Sales
{
    public static class ConfigureServices
    {
        public static void AddSales(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<SalesDbContext>();
            serviceCollection.AddMediatR(typeof(ConfigureServices).Assembly);
        }
    }
}
