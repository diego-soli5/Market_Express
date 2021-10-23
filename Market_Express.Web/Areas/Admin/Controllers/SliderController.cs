using AutoMapper;
using Market_Express.Application.DTOs.Slider;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "SLI_MAN_GEN")]
    public class SliderController : BaseController
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
            var oResult = await _sliderService.Create(model.Name, model.Image, CurrentUserId);

            if (oResult.Success)
            {
                TempData["SliderMessage"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            if (oResult.ResultCode == 1)
            {
                ModelState.AddModelError("Name", oResult.Message);
            }
            else if (oResult.ResultCode == 2)
            {
                ModelState.AddModelError("Image", oResult.Message);
            }

            return View(model);
        }

        [HttpGet]
        [Route("Admin/Slider/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var oSlider = await _sliderService.GetById(id);

            return View(_mapper.Map<SliderUpdateDTO>(oSlider));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SliderUpdateDTO model)
        {
            var oSlider = _mapper.Map<Slider>(model);

            var oResult = await _sliderService.Update(oSlider, model.NewImage, CurrentUserId);

            if (!oResult.Success)
            {
                if (oResult.ResultCode == 1)
                {
                    ModelState.AddModelError("Name", oResult.Message);
                }
                else if (oResult.ResultCode == 2)
                {
                    ModelState.AddModelError("NewImage", oResult.Message);
                }
                else if (oResult.ResultCode == 3)
                {
                    ViewData["Message"] = oResult.Message;
                }

                return View(model);
            }

            TempData["SliderMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromQuery(Name = "id")] Guid id)
        {
            var oResult = await _sliderService.ChangeStatus(id, CurrentUserId);

            return Ok(oResult);
        }
        #endregion
    }
}
