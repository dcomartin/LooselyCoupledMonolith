using System;

namespace Sales
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        InProgress = 0,
        Placed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4
    }
}