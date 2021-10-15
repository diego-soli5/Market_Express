using Market_Express.Application.DTOs.Category;

namespace Market_Express.Web.ViewModels.Category
{
    public class CategoryDetailsViewModel
    {
        public CategoryDTO Category { get; set; }
        public int ArticlesEnabledCount { get; set; }
        public int ArticlesDisabledCount { get; set; }

        public int TotalArticlesCount => ArticlesEnabledCount + ArticlesDisabledCount;
    }
}
