using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Article
{
    public class ArticleEditDTO
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool AutoSync { get; set; }
        public bool AutoSyncDescription { get; set; }
        public EntityStatus Status { get; set; }
        public string AddedBy { get; set; }
    }
}
