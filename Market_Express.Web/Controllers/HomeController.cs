using AutoMapper;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
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

        #region MISCELLANEOUS VIEWS

        #region COMPANY VIEWS
        [HttpGet]
        [Route("/Home/Company/About")]
        public IActionResult AboutCompany()
        {
            return View();
        }
        #endregion

        #region HELP VIEWS
        [HttpGet]
        [Route("/Home/Help/Contact")]
        public IActionResult ContactHelp()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/Help/UserManual")]
        public IActionResult UserManual()
        {
            return View();
        }
        #endregion

        #region SYSTEM VIEWS
        [HttpGet]
        [Route("/Home/System/About")]
        public IActionResult AboutSystem()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/System/Contact")]
        public IActionResult ContactSystem()
        {
            return View();
        }
        #endregion

        #endregion
    }
}
