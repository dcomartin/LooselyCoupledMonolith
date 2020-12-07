using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Sales.Contracts;
using Shipping.Contracts;

namespace Shipping
{
    public class CreateShippingLabel : ICapSubscribe
    {
        private readonly ICapPublisher _publisher;
        private readonly ShippingDbContext _dbContext;

        public CreateShippingLabel(ICapPublisher publisher, ShippingDbContext dbContext)
        {
            _publisher = publisher;
            _dbContext = dbContext;
        }

        [CapSubscribe(nameof(OrderPlaced))]
        public async Task Handle(OrderPlaced orderPlaced, [FromCap]CapHeader header)
        {
            var messageId = header.GetMessageId();
            if (await _dbContext.HasBeenProcessed(messageId, nameof(CreateShippingLabel)))
            {
                return;
            }

            using (var trx = _dbContext.Database.BeginTransaction(_publisher))
            {
                await _dbContext.ShippingLabels.AddAsync(new ShippingLabel
                {
                    OrderId = orderPlaced.OrderId,
                    OrderDate = DateTime.UtcNow
                });
                await _dbContext.SaveChangesAsync();

                await _dbContext.IdempotentConsumer(messageId, nameof(CreateShippingLabel));

                await _publisher.PublishAsync(nameof(ShippingLabelCreated), new ShippingLabelCreated
                {
                    OrderId = orderPlaced.OrderId
                });

                await trx.CommitAsync();
            }

        }
    }
}