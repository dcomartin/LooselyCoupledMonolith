using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Shipping.Contracts;

namespace Sales
{
    public class ChangeOrderStatus : IHandleMessages<ShippingLabelCreated>
    {
        private readonly SalesDbContext _dbContext;

        public ChangeOrderStatus(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(ShippingLabelCreated message, IMessageHandlerContext context)
        {
            var order = await _dbContext.Orders.SingleAsync(x => x.OrderId == message.OrderId);
            order.Status = OrderStatus.ReadyToShip;
            await _dbContext.SaveChangesAsync();
        }
    }
}