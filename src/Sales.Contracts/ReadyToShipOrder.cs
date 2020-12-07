using System;
using NServiceBus;

namespace Sales.Contracts
{
    public class ReadyToShipOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}