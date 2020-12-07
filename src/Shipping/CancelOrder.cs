using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Sales.Contracts;

namespace Shipping
{
    public class CancelShippingLabel : IHandleMessages<OrderCancelled>
    {
        private readonly ShippingDbContext _dbContext;

        public CancelShippingLabel(ShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(OrderCancelled message, IMessageHandlerContext context)
        {
            var order = await _dbContext.ShippingLabels.SingleAsync(x => x.OrderId == message.OrderId);
            order.Cancelled = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}