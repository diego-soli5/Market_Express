using System;

namespace Market_Express.Domain.CustomEntities.Order
{
    public class OrderArticleDetail
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
