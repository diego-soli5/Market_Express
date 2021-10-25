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
        [Route("/Admin/Role/Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            RoleEditViewModel oViewModel = new();

            var oRoleWithPermissions = await _roleService.GetByIdWithPermissions(id);

            oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();

            var lstPermissions = _roleService.GetAllPermissions();

            oViewModel.Role = _mapper.Map<RoleDTO>(oRoleWithPermissions);

            oRoleWithPermissions.Permissions.ToList().ForEach(permission =>
            {
                oViewModel.Role.Permissions.Add(_mapper.Map<PermissionDTO>(permission));
            });

            lstPermissions.ForEach(per =>
            {
                oViewModel.Permissions.Add(_mapper.Map<PermissionDTO>(per));
            });

            return View(oViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleDTO model, List<Guid> permissions)
        {
            Role oRole = _mapper.Map<Role>(model);

            var oResult = await _roleService.Edit(oRole, permissions, CurrentUserId);

            if (!oResult.Success)
            {
                RoleEditViewModel oViewModel = new();

                oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();

                var lstPermissions = _roleService.GetAllPermissions();

                oViewModel.Role = model;

                foreach(var per in permissions)
                {
                    oViewModel.Role.Permissions
                              .Add(_mapper.Map<PermissionDTO>(await _roleService.GetPermissionById(per)));
                }

                lstPermissions.ForEach(per =>
                {
                    oViewModel.Permissions.Add(_mapper.Map<PermissionDTO>(per));
                });

                ViewData["MessageResult"] = oResult.Message;

                return View(oViewModel);
            }

            TempData["RoleMessage"] = oResult.Message;

            return RedirectToAction(nameof(Index));
        }

        #region UTILITY METHODS
        private async Task<RoleEditViewModel> CreateRoleEditViewModel(Guid id)
        {
            return null;
        }
        #endregion
    }
}
