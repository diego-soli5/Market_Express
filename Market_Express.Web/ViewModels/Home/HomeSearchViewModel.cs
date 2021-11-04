using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.QueryFilter.Home;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Home
{
    public class HomeSearchViewModel
    {
        public HomeSearchViewModel()
        {
            Filters = new();
            Categories = new();
            Articles = new();
        }

        public HomeSearchQueryFilter Filters { get; set; }
        public List<CategorySearchDTO> Categories { get; set; }
        public List<ArticleDTO> Articles { get; set; }
    }
}
