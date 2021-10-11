using AutoMapper;
using Market_Express.Application.DTOs.Category;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
    }
}
