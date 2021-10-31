using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleDTO
    {
        public Guid? Id { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool AutoSync { get; set; }
        public bool AutoSyncDescription { get; set; }
        public EntityStatus Status { get; set; }

        public CategoryDTO Category { get; set; }
    }
}
