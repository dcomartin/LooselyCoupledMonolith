using System.Threading.Tasks;
using Billing.Contracts;
using NServiceBus;

namespace Billing
{
    public class RefundOrderHandler : IHandleMessages<RefundOrder>
    {
        public async Task Handle(RefundOrder message, IMessageHandlerContext context)
        {
            await context.Publish<OrderRefunded>(refunded =>
            {
                refunded.OrderId = message.OrderId;
            });
        }
    }
}