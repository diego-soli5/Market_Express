using AutoMapper;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Abstractions.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;

        public SliderController(ISliderService sliderService,
                                IMapper mapper)
        {
            _sliderService = sliderService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstSlider = _sliderService.GetAll();

            List<SliderDTO> lstSliderDTO = new();

            lstSlider?.ToList().ForEach(slider =>
            {
                lstSliderDTO.Add(_mapper.Map<SliderDTO>(slider));
            });

            return View(lstSliderDTO);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateDTO model)
        {
            var oResult = await _sliderService.Create(model.Name, model.Image);

            if (oResult.Success)
            {
                TempData["SliderMessage"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            if (oResult.Message_Code == 1)
            {
                ModelState.AddModelError("Name", oResult.Message);
            }
            else if (oResult.Message_Code == 2)
            {
                ModelState.AddModelError("Image", oResult.Message);
            }

            return View(new SliderDTO(model.Name));
        }
    }
}
