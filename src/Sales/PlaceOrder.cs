using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Sales.Contracts;

namespace Sales
{
    public class PlaceOrderController : Controller
    {
        private readonly SalesDbContext _dbContext;
        private readonly IMessageSession _messageSession;

        public PlaceOrderController(SalesDbContext dbContext, IMessageSession messageSession)
        {
            _dbContext = dbContext;
            _messageSession = messageSession;
        }

        [HttpPost("/sales/orders/{orderId:Guid}")]
        public async Task<IActionResult> Action([FromRoute] Guid orderId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
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
                await _messageSession.Publish(orderPlaced);

                await transaction.CommitAsync();
            }

            return NoContent();
        }
    }

}