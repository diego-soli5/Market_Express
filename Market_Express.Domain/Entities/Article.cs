using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Article : BaseEntity
    {
        public Article()
        {
            CartDetails = new HashSet<CartDetail>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool AutoSync { get; set; }
        public bool AutoSyncDescription { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public Category Category { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
