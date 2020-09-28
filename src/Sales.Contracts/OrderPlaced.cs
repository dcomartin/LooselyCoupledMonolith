using System;
using MediatR;

namespace Sales.Contracts
{
    public class OrderPlaced : INotification
    {
        public Guid OrderId { get; set; }
    }
}