using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Sales.Contracts;

namespace Sales
{
    public class PlaceOrderController : Controller
    {
        private readonly SalesDbContext _dbContext;
        private readonly ICapPublisher _publisher;

        public PlaceOrderController(SalesDbContext dbContext, ICapPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        [HttpPost("/sales/orders/{orderId:Guid}")]
        public async Task<IActionResult> Action([FromRoute] Guid orderId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction(_publisher))
            {
                await _dbContext.Orders.AddAsync(new Order
                {
                    OrderId = orderId,
                    Status = OrderStatus.Pending
                });
                await _dbContext.SaveChangesAsync();

                var orderPlaced = new OrderPlaced
                {
                    OrderId = orderId
                };
                await _publisher.PublishAsync(nameof(OrderPlaced), orderPlaced);

                await transaction.CommitAsync();
            }

            return NoContent();
        }
    }

}