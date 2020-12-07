using System;
using NServiceBus;

namespace Sales.Contracts
{
    public class CancelOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}