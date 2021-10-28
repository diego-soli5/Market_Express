using AutoMapper;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Application.DTOs.Role;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Web.Controllers;
using Market_Express.Web.ViewModels.Users;
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
    [Authorize(Roles = "USE_MAN_GEN")]
    public class UsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UsersController(IAppUserService appUserService,
                               IRoleService roleService,
                               IMapper mapper)
        {
            _appUserService = appUserService;
            _roleService = roleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lstAppUserDTO = GetAppUserDTOList();

            return View(lstAppUserDTO);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UserCreateViewModel oViewModel = new();

            oViewModel.AvailableRoles = GetRoleDTOList();

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUserCreateDTO model)
        {
            AppUser oUser = new(model.Name, model.Identification, model.Email, model.Phone, model.Type);

            var oResult = await _appUserService.Create(oUser, model.Roles, CurrentUserId);

            if (!oResult.Success)
            {
                UserCreateViewModel oViewModel = new();

                oViewModel.AvailableRoles = GetRoleDTOList();
                oViewModel.AppUser = model;

                if (oResult.ResultCode == 0)
                    ModelState.AddModelError("AppUser.Identification", oResult.Message);
                else if (oResult.ResultCode == 1)
                    ModelState.AddModelError("AppUser.Email", oResult.Message);
                else if (oResult.ResultCode == 2)
                    ModelState.AddModelError("AppUser.Phone", oResult.Message);
                else
                    ViewData["MessageResult"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["UserMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> ChangeStatus([FromQuery(Name = "id")] Guid id)
        {
            var oResult = await _appUserService.ChangeStatus(id, CurrentUserId);

            return Ok(oResult);
        }
        #endregion

        #region UTILITY METHODS
        private List<AppUserDTO> GetAppUserDTOList()
        {
            List<AppUserDTO> lstAppUserDTO = new();

            _appUserService.GetAll().ToList()?.ForEach(user =>
            {
                lstAppUserDTO.Add(_mapper.Map<AppUserDTO>(user));
            });

            return lstAppUserDTO;
        }

        private List<RoleDTO> GetRoleDTOList()
        {
            List<RoleDTO> lstRoleDTO = new();

            var lstRole = _roleService.GetAll();

            lstRole.ForEach(role =>
            {
                lstRoleDTO.Add(_mapper.Map<RoleDTO>(role));
            });

            return lstRoleDTO;
        }
        #endregion
    }
}
