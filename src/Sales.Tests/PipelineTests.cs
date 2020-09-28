using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sales.Tests
{
    public class PipelineTests : IAsyncLifetime
    {
        private IMediator _mediator;
        private ServiceProvider _provider;
        private Guid _customerId;
        private Guid _orderId;

        public PipelineTests()
        {
            _customerId = Guid.NewGuid();
            _orderId = Guid.NewGuid();

            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddSales();

            _provider = sc.BuildServiceProvider();
            _mediator = _provider.GetService<IMediator>();
        }

        public async Task InitializeAsync()
        {
            var db = _provider.GetService<SalesDbContext>();
            db.Orders.Add(new Order
            {
                OrderId = _orderId,
                Status = OrderStatus.InProgress
            });
            await db.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _provider.DisposeAsync();
        }

        [Fact]
        public async Task Test()
        {
            await _mediator.Send(new PlaceOrder
            {
                OrderId = _orderId
            });
        }


    }
}