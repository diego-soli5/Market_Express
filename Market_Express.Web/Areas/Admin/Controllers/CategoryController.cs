using AutoMapper;
using Market_Express.Application.DTOs.Category;
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
    [Authorize(Roles = "ADMINISTRADOR")]
    [Area("Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService,
                                  IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstCategory = _categoryService.GetAll();

            List<CategoryDTO> lstCategoryDTO = new();

            lstCategory?.ToList().ForEach(slider =>
            {
                lstCategoryDTO.Add(_mapper.Map<CategoryDTO>(slider));
            });

            return View(lstCategoryDTO);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            Category oCategory = new(model.Name,model.Description);

            var oResult = await _categoryService.Create(oCategory, model.Image, CurrentUserId);

            if (oResult.Success)
            {
                TempData["CategoryMessage"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            if (oResult.ResultCode == 1)
            {
                ViewData["MessageResult"] = oResult.Message;
            }
            else if (oResult.ResultCode == 2)
            {
                ModelState.AddModelError("Image", oResult.Message);
            }

            return View(model);
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromQuery(Name = "id")] Guid id)
        {
            var oResult = await _categoryService.ChangeStatus(id, CurrentUserId);

            return Ok(oResult);
        }
        #endregion
    }
}
