using System;
using Paramore.Brighter;

namespace Sales.Contracts
{
    public class OrderPlaced : IEvent
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
    }
}