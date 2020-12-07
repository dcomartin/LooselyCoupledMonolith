using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Sales.Contracts;
using Shipping.Contracts;

namespace Sales
{
    public class ReadyToShipHandler :
        IHandleMessages<ShippingLabelCreated>,
        IHandleMessages<ReadyToShipOrder>
    {
        private readonly SalesDbContext _dbContext;

        public ReadyToShipHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task SetStatus(Guid orderId)
        {
            var order = await _dbContext.Orders.SingleAsync(x => x.OrderId == orderId);
            order.Status = OrderStatus.ReadyToShip;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Handle(ShippingLabelCreated message, IMessageHandlerContext context)
        {
            await SetStatus(message.OrderId);
        }

        public async Task Handle(ReadyToShipOrder message, IMessageHandlerContext context)
        {
            await SetStatus(message.OrderId);
        }
    }
}