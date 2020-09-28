using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Contracts;

namespace Shipping
{
    public class CreateShippingLabel : INotificationHandler<OrderPlaced>
    {
        private readonly ILogger<CreateShippingLabel> _logger;

        public CreateShippingLabel(ILogger<CreateShippingLabel> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Order {notification.OrderId} was placed... lets create a new shipping label.");
            return Task.CompletedTask;
        }
    }
}