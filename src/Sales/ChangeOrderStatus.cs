using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Shipping.Contracts;

namespace Sales
{
    public class ChangeOrderStatus : ICapSubscribe
    {
        private readonly SalesDbContext _dbContext;

        public ChangeOrderStatus(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [CapSubscribe(nameof(ShippingLabelCreated))]
        public async Task Handle(ShippingLabelCreated shippingLabelCreated)
        {
            var order = await _dbContext.Orders.SingleAsync(x => x.OrderId == shippingLabelCreated.OrderId);
            order.Status = OrderStatus.ReadyToShip;
            await _dbContext.SaveChangesAsync();
        }
    }
}