using System;
using NServiceBus;

namespace Sales.Contracts
{
    public class OrderPlaced : IEvent
    {
        public Guid OrderId { get; set; }
    }
}