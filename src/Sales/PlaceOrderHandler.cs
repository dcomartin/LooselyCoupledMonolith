using System;
using System.Threading.Tasks;
using Billing.Contracts;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Sales.Contracts;
using Shipping.Contracts;

namespace Sales
{
    public class PlaceOrderController : Controller
    {
        private readonly IMessageSession _messageSession;

        public PlaceOrderController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [HttpPost("/sales/orders/{orderId:Guid}")]
        public async Task<IActionResult> Action([FromRoute] Guid orderId)
        {
            await _messageSession.Send(new PlaceOrder
            {
                OrderId = orderId
            });

            return NoContent();
        }
    }

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private readonly SalesDbContext _dbContext;

        public PlaceOrderHandler(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            await _dbContext.Orders.AddAsync(new Order
            {
                OrderId = message.OrderId,
                Status = OrderStatus.Pending
            });
            await _dbContext.SaveChangesAsync();

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            await context.Publish(orderPlaced);
        }
    }

    public class PlaceOrderSagaData : ContainSagaData
    {
        public Guid OrderId { get; set; }
        public bool HasBeenBilled { get; set; }
    }

    public class PlaceOrderSaga :
        Saga<PlaceOrderSagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IHandleMessages<OrderBilled>,
        IHandleMessages<ShippingLabelCreated>,
        IHandleMessages<Backordered>,
        IHandleMessages<OrderRefunded>
    {

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<PlaceOrderSagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<ShippingLabelCreated>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<Backordered>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderRefunded>(message => message.OrderId).ToSaga(sagaData => sagaData.OrderId);
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            await context.Send(new BillOrder
            {
                OrderId = message.OrderId
            });
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.HasBeenBilled = true;

            await context.Send(new CreateShippingLabel
            {
                OrderId = message.OrderId
            });
        }

        public async Task Handle(ShippingLabelCreated message, IMessageHandlerContext context)
        {
            await context.SendLocal(new ReadyToShipOrder
            {
                OrderId = Data.OrderId
            });
            MarkAsComplete();
        }

        public async Task Handle(Backordered message, IMessageHandlerContext context)
        {
            if (Data.HasBeenBilled)
            {
                await context.Send(new RefundOrder()
                {
                    OrderId = message.OrderId
                });
            }
            else
            {
                MarkAsComplete();
            }
        }

        public async Task Handle(OrderRefunded message, IMessageHandlerContext context)
        {
            await context.SendLocal(new CancelOrder
            {
                OrderId = message.OrderId
            });
            MarkAsComplete();
        }
    }
}