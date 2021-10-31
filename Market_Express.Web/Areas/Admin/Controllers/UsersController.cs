using AutoMapper;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Application.DTOs.Client;
using Market_Express.Application.DTOs.Role;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.AppUser;
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

        [HttpGet]
        public IActionResult Index(AppUserIndexQueryFilter filters)
        {
            UserIndexViewModel oViewModel = new();

            oViewModel.AppUsers = GetAppUserDTOList(filters);
            oViewModel.QueryFilter = filters;

            return View(oViewModel);
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

        [HttpGet]
        [Route("/Admin/Users/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            UserEditViewModel oViewModel = new();

            var tplUserDTOAndClientDTO = await GetAppUserEditDTO(id);

            oViewModel.AvailableRoles = GetRoleDTOList();
            oViewModel.Client = tplUserDTOAndClientDTO.Item2;
            oViewModel.AppUser = tplUserDTOAndClientDTO.Item1;
            oViewModel.AppUser.Roles = await GetAppUserRoleIdList(id);

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppUserEditDTO userModel, ClientDTO clientModel)
        {
            AppUser oUser = new(userModel.Id, userModel.Identification, userModel.Email, userModel.Phone, userModel.Status, userModel.Type);
            oUser.Client = new(clientModel.AutoSync, clientModel.ClientCode);

            var oResult = await _appUserService.Edit(oUser, userModel.Roles, CurrentUserId);

            if (!oResult.Success)
            {
                UserEditViewModel oViewModel = new();

                oViewModel.AppUser = userModel;
                oViewModel.Client = clientModel;
                oViewModel.AvailableRoles = GetRoleDTOList();

                ViewData["MessageResult"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["UserMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("/Admin/Users/Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var oAppUser = await GetAppUserDetailsDTO(id);

            return View(oAppUser);
        }

        [HttpGet]
        public IActionResult GetUserTable(AppUserIndexQueryFilter filters)
        {
            return PartialView("_appUserTablePartial", GetAppUserDTOList(filters));
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
        private async Task<AppUserDetailsDTO> GetAppUserDetailsDTO(Guid id)
        {
            var oAppUser = await _appUserService.GetById(id, true);

            var oAppUserDetailsDTO = _mapper.Map<AppUserDetailsDTO>(oAppUser);

            oAppUserDetailsDTO.IsInPOS = oAppUser.Client.ClientCode != null;

            if (oAppUserDetailsDTO.Type == AppUserType.ADMINISTRADOR)
                oAppUserDetailsDTO.Roles = await GetAppUserRoleList(id);

            return oAppUserDetailsDTO;
        }

        private async Task<List<RoleDTO>> GetAppUserRoleList(Guid id)
        {
            List<RoleDTO> lstRoleDTO = new();

            var lstRoles = await _roleService.GetAllByUserId(id);

            lstRoles.ForEach(r =>
            {
                lstRoleDTO.Add(_mapper.Map<RoleDTO>(r));
            });

            return lstRoleDTO;
        }

        private async Task<List<Guid>> GetAppUserRoleIdList(Guid id)
        {
            List<Guid> lstRoleGuid = new();

            var lstRoles = await _roleService.GetAllByUserId(id);

            lstRoles.ForEach(r =>
            {
                lstRoleGuid.Add(r.Id);
            });

            return lstRoleGuid;
        }

        private async Task<(AppUserEditDTO, ClientDTO)> GetAppUserEditDTO(Guid id)
        {
            var oAppUser = await _appUserService.GetById(id, true);

            return (_mapper.Map<AppUserEditDTO>(oAppUser), _mapper.Map<ClientDTO>(oAppUser.Client));
        }

        private List<AppUserDTO> GetAppUserDTOList(AppUserIndexQueryFilter filters)
        {
            List<AppUserDTO> lstAppUserDTO = new();

            _appUserService.GetAll(filters).ToList()?.ForEach(user =>
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
