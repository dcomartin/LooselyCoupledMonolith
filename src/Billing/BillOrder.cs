using System.Threading.Tasks;
using Billing.Contracts;
using NServiceBus;
using Sales.Contracts;

namespace Billing
{
    public class BillOrder : IHandleMessages<OrderPlaced>
    {
        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            await context.Publish(new OrderBilled
            {
                OrderId = message.OrderId
            });
        }
    }
}