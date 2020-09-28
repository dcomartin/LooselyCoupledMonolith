using System;
using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;
using Paramore.Brighter.Policies.Attributes;

namespace Sales
{
    public class OrdersController : Controller
    {
        private readonly IAmACommandProcessor _commandProcessor;

        public OrdersController(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        [HttpPost("/sales/orders/{orderId:Guid}")]
        public IActionResult PlaceOrder([FromRoute]Guid orderId)
        {
            var request = new PlaceOrder
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
            };

            _commandProcessor.Post(request);

            return NoContent();
        }
    }

    public class PlaceOrder : ICommand
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
    }

    public class PlaceOrderHandler : RequestHandler<PlaceOrder>
    {
        private readonly IAmACommandProcessor _commandProcessor;

        public PlaceOrderHandler(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        [UsePolicy(CommandProcessor.RETRYPOLICY, 3)]
        public override PlaceOrder Handle(PlaceOrder command)
        {
            throw new InvalidOperationException();

            return base.Handle(command);
        }
    }
}