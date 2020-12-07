using System;
using NServiceBus;

namespace Billing.Contracts
{
    public class OrderBilled : IEvent
    {
        public Guid OrderId { get; set; }
    }
}