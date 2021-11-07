using System;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleToAddInCartDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int CountInCart { get; set; }
    }
}
