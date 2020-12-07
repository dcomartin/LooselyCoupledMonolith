using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Sales.Contracts;

namespace Shipping
{
    public class CancelShippingLabel : ICapSubscribe
    {
        private readonly ShippingDbContext _dbContext;

        public CancelShippingLabel(ShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe(nameof(OrderPlaced))]
        public async Task Handle(OrderCancelled orderCancelled)
        {
            var order = await _dbContext.ShippingLabels.SingleAsync(x => x.OrderId == orderCancelled.OrderId);
            order.Cancelled = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}