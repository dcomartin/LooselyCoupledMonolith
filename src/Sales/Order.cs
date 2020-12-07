using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        ReadyToShip = 1,
        Shipped = 2,
        Cancelled = 3
    }
}