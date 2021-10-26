using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Market_Express.Domain.Abstractions.DomainServices;
using AutoMapper;
using System.Collections.Generic;
using Market_Express.Application.DTOs.Role;
using System;
using Market_Express.Web.ViewModels.Role;
using System.Threading.Tasks;
using System.Linq;
using Market_Express.Application.DTOs.Permission;
using Market_Express.Domain.Entities;
using Market_Express.Web.Controllers;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "ROL_MAN_GEN")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService,
                              IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var lstRoles = _roleService.GetAll();

            List<RoleDTO> lstRoleDTO = new();

            lstRoles.ForEach(role =>
            {
                lstRoleDTO.Add(_mapper.Map<RoleDTO>(role));
            });

            return View(lstRoleDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            RoleCreateViewModel oViewModel = new();

            oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();
            oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDTO model)
        {
            Role oRole = new(model.Name, model.Description);

            var oResult = await _roleService.Create(oRole, model.Permissions, CurrentUserId);

            if (!oResult.Success)
            {
                RoleCreateViewModel oViewModel = new();

                oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();
                oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();
                oViewModel.Role = model;

                ViewData["MessageResult"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["RoleMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("/Admin/Role/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            RoleEditViewModel oViewModel = new();

            var oRoleWithPermissions = await _roleService.GetByIdWithPermissions(id);

            oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();

            oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();

            oViewModel.Role = _mapper.Map<RoleDTO>(oRoleWithPermissions);

            oRoleWithPermissions.Permissions.ToList().ForEach(permission =>
            {
                oViewModel.Role.Permissions.Add(_mapper.Map<PermissionDTO>(permission));
            });

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleDTO model, List<Guid> permissions)
        {
            Role oRole = new(model.Id, model.Name, model.Description);

            var oResult = await _roleService.Edit(oRole, permissions, CurrentUserId);

            if (!oResult.Success)
            {
                RoleEditViewModel oViewModel = new();

                oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();

                oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();

                oViewModel.Role = model;

                foreach (var per in permissions)
                {
                    oViewModel.Role.Permissions
                              .Add(_mapper.Map<PermissionDTO>(await _roleService.GetPermissionById(per)));
                }

                ViewData["MessageResult"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["RoleMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery(Name = "Id")] Guid id)
        {
            var oResult = await _roleService.Delete(id, CurrentUserId);
            
            return Ok(oResult);
        }
        #endregion

        #region UTILITY METHODS
        private List<PermissionDTO> GetAllPermissionsAvailable()
        {
            List<PermissionDTO> lstPermissionsDTO = new();

            var lstPermissions = _roleService.GetAllPermissions();

            lstPermissions.ForEach(per =>
            {
                lstPermissionsDTO.Add(_mapper.Map<PermissionDTO>(per));
            });

            return lstPermissionsDTO;
        }
        #endregion
    }
}
