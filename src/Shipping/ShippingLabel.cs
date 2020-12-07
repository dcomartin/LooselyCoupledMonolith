using System;
using System.ComponentModel.DataAnnotations;

namespace Shipping
{
    public class ShippingLabel
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Cancelled { get; set; }
    }
}