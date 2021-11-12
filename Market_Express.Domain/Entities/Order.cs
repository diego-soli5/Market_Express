using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid ClientId { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; }

        public Client Client { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
