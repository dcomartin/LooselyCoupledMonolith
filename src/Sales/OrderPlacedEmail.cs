using System;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;
using Sales.Contracts;

namespace Sales
{
    public class OrderPlacedEmail : RequestHandler<OrderPlaced>
    {
        private readonly ILogger<OrderPlacedEmail> _logger;

        public OrderPlacedEmail(ILogger<OrderPlacedEmail> logger)
        {
            _logger = logger;
        }

        public override OrderPlaced Handle(OrderPlaced command)
        {
            throw new InvalidOperationException();

            return base.Handle(command);
        }
    }
}