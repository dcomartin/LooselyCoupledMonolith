using System;
using NServiceBus;

namespace Billing.Contracts
{
    public class BillOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}