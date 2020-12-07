using System;
using NServiceBus;

namespace Shipping.Contracts
{
    public class CreateShippingLabel : ICommand
    {
        public Guid OrderId { get; set; }
    }
}