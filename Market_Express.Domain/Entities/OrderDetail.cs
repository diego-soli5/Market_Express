using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ArticleId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public Article Article { get; set; }
        public Order Order { get; set; }
    }
}
