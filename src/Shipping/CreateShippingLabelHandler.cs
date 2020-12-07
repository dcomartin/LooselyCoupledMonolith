using System;
using System.Threading.Tasks;
using Billing.Contracts;
using NServiceBus;
using Sales.Contracts;
using Shipping.Contracts;

namespace Shipping
{
    public class CreateShippingLabelHandler :
        //IHandleMessages<OrderBilled>,
        IHandleMessages<CreateShippingLabel>
    {
        private readonly ShippingDbContext _dbContext;

        public CreateShippingLabelHandler(ShippingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            await _dbContext.ShippingLabels.AddAsync(new ShippingLabel
            {
                OrderId = message.OrderId,
                OrderDate = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            await context.Publish<ShippingLabelCreated>(created =>
            {
                created.OrderId = message.OrderId;
            });
        }

        public async Task Handle(CreateShippingLabel message, IMessageHandlerContext context)
        {
            await _dbContext.ShippingLabels.AddAsync(new ShippingLabel
            {
                OrderId = message.OrderId,
                OrderDate = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            await context.Publish<ShippingLabelCreated>(created =>
            {
                created.OrderId = message.OrderId;
            });

            /*
            await context.Publish<Backordered>(backordered =>
            {
                backordered.OrderId = message.OrderId;
            });
            */
        }
    }

    public class CreateShippingLabelSagaData : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }

    public class CreateShippingLabelSaga : Saga<CreateShippingLabelSagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CreateShippingLabelSagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
        }

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new CreateShippingLabel() { OrderId = Data.OrderId });
                MarkAsComplete();
            }
        }
    }
}