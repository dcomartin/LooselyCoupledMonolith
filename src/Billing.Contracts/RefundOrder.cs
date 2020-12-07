using System;
using NServiceBus;

namespace Billing.Contracts
{
    public class RefundOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}