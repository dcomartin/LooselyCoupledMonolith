using System;
using NServiceBus;

namespace Sales.Contracts
{
    public class OrderCancelled : IEvent
    {
        public Guid OrderId { get; set; }
    }
}