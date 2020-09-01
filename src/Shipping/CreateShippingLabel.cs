using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using Sales.Contracts;

namespace Shipping
{
    public class CreateShippingLabel : ICapSubscribe
    {
        private readonly ILogger<CreateShippingLabel> _logger;

        public CreateShippingLabel(ILogger<CreateShippingLabel> logger)
        {
            _logger = logger;
        }

        [CapSubscribe(nameof(OrderPlaced))]
        public void Handle(OrderPlaced orderPlaced)
        {
            _logger.LogInformation($"Order {orderPlaced.OrderId} was placed... lets create a new shipping label.");
        }
    }
}