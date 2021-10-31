using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Article;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Article
{
    public class ArticleIndexViewModel
    {
        public ArticleIndexViewModel()
        {
            QueryFilter = new();
            Articles = new();
            Metadata = new();
        }

        public ArticleIndexQueryFilter QueryFilter { get; set; }
        public List<ArticleDTO> Articles { get; set; }
        public List<CategoryDTO> AvailableCategories { get; set; }
        public Metadata Metadata { get; set; }
    }
}
