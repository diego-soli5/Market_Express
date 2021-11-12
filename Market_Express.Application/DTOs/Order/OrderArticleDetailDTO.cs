using System;

namespace Market_Express.Application.DTOs.Order
{
    public class OrderArticleDetailDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Total => Price * Quantity;
    }
}
