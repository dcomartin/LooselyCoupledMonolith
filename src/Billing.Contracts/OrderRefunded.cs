using System;
using NServiceBus;

namespace Billing.Contracts
{
    public class OrderRefunded : IEvent
    {
        public Guid OrderId { get; set; }
    }
}