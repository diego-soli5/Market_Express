using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Domain.QueryFilter.Article
{
    public class ArticleIndexQueryFilter : PaginationQueryFilter
    {
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public EntityStatus? Status { get; set; }
        public bool ImgIsNull { get; set; }
        public bool CategoryIdIsNull { get; set; }
    }
}
