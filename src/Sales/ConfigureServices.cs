using Microsoft.Extensions.DependencyInjection;

namespace Sales
{
    public static class ConfigureServices
    {
        public static void AddSales(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<SalesDbContext>();
        }
    }
}
