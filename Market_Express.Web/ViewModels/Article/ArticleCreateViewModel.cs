using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Article
{
    public class ArticleCreateViewModel
    {
        public ArticleCreateViewModel()
        {
            Article = new();
            AvailableCategories = new();
        }

        public ArticleCreateDTO Article { get; set; }
        public List<CategoryDTO> AvailableCategories { get; set; }
    }
}
