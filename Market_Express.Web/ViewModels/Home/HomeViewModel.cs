using Market_Express.Application.DTOs.Article;
using Market_Express.Application.DTOs.Category;
using Market_Express.Application.DTOs.Slider;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Sliders = new();
            Categories = new();
            PopularArticles = new();
        }

        public List<SliderDTO> Sliders { get; set; }
        public List<CategoryDTO> Categories { get; set; }
        public List<ArticleDTO> PopularArticles { get; set; }
    }
}
