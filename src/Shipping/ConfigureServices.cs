using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shipping
{
    public static class ConfigureServices
    {
        public static void AddShipping(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CreateShippingLabel>();
            serviceCollection.AddMediatR(typeof(ConfigureServices).Assembly);
        }
    }
}
