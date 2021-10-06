using AutoMapper;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.ViewModels;
using Market_Express.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Market_Express.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeService homeService,
                              IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            HomeViewModel oViewModel = new();

            var lstSliders = _homeService.GetAllSliders();

            lstSliders?.ToList().ForEach(slider =>
            {
                oViewModel.Sliders.Add(_mapper.Map<SliderDTO>(slider));
            });

            return View(oViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
