using Microsoft.Extensions.DependencyInjection;

namespace Shipping
{
    public static class ConfigureServices
    {
        public static void AddShipping(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ShippingDbContext>();
            serviceCollection.AddTransient<CreateShippingLabelHandler>();
        }
    }
}
