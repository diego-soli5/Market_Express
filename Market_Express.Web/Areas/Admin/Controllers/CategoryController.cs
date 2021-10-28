using AutoMapper;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Web.Controllers;
using Market_Express.Web.ViewModels.Category;
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
    [Authorize(Roles = "CAT_MAN_GEN")]
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            Category oCategory = new(model.Name, model.Description);

            var oResult = await _categoryService.Create(oCategory, model.Image, CurrentUserId);

            if (oResult.Success)
            {
                TempData["CategoryMessage"] = oResult.Message;

                return RedirectToAction(nameof(Index));
            }

            if (oResult.ResultCode == 1)
                ViewData["MessageResult"] = oResult.Message;

            else if (oResult.ResultCode == 2)
                ModelState.AddModelError("Image", oResult.Message);


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            CategoryUpdateDTO categoryUpdateDTO = _mapper.Map<CategoryUpdateDTO>(await _categoryService.GetById(id));

            return View(categoryUpdateDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateDTO model)
        {
            Category oCategory = new(model.Id, model.Name, model.Description, model.Status);

            var oResult = await _categoryService.Edit(oCategory, model.NewImage, CurrentUserId);

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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            CategoryDetailsViewModel oViewModel = new();

            var oCategory = await _categoryService.GetById(id);
            var tpArticleDetails = await _categoryService.GetArticleDetails(id);

            oViewModel.ArticlesEnabledCount = tpArticleDetails.Item1;
            oViewModel.ArticlesDisabledCount = tpArticleDetails.Item2;
            oViewModel.Category = _mapper.Map<CategoryDTO>(oCategory);

            return View(oViewModel);
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
