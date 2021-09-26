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
        public string Status { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
