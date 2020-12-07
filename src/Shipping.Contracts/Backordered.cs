using System;
using NServiceBus;

namespace Shipping.Contracts
{
    public class Backordered : IEvent
    {
        public Guid OrderId { get; set; }
    }
}