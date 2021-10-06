using Market_Express.Application.DTOs.Slider;
using System.Collections.Generic;

namespace Market_Express.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Sliders = new();
        }

        public List<SliderDTO> Sliders { get; set; }
    }
}
