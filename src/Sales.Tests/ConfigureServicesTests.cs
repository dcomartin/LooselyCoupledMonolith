using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Sales.Tests
{
    public class ConfigureServicesTests
    {
        [Fact]
        public void DbContext_should_resolve()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSales();

            var provider = serviceCollection.BuildServiceProvider();
            var dbContext = provider.GetService<SalesDbContext>();
            dbContext.ShouldNotBeNull();
        }
    }
}
