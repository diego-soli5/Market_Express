using Microsoft.AspNetCore.Http;
using System;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleCreateDTO
    {
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
