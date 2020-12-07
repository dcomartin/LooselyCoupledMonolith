using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Sales.Contracts;

namespace Sales
{
    public class CancelOrderHandler : IHandleMessages<CancelOrder>
    {
        private readonly SalesDbContext _dbContext;

        public CancelOrderHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            var order = await _dbContext.Orders.SingleAsync(x => x.OrderId == message.OrderId);
            order.Status = OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync();

            await context.Publish<OrderCancelled>(cancelled =>
            {
                cancelled.OrderId = message.OrderId;
            });
        }
    }
}