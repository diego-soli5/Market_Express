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
            List<RoleDTO> lstRoleDTO = GetRoleDTOList();

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

            var tplRoleDTOWithPermissionsDTOList = await GetRoleDTOWithPermissionDTOListByRoleId(id);

            oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();
            oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();
            oViewModel.Role = tplRoleDTOWithPermissionsDTOList.Item1;
            oViewModel.Role.Permissions = tplRoleDTOWithPermissionsDTOList.Item2;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            RoleDetailsViewModel oViewModel = new();

            var tplRoleDTOWithPermissionsDTOList = await GetRoleDTOWithPermissionDTOListByRoleId(id);

            oViewModel.PermissionTypes = await _roleService.GetAllPermissionTypes();
            oViewModel.PermissionsAvailable = GetAllPermissionsAvailable();
            oViewModel.Role = tplRoleDTOWithPermissionsDTOList.Item1;

            return View(oViewModel);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetTable()
        {
            return PartialView("_roleTablePartial", GetRoleDTOList());
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery(Name = "Id")] Guid id)
        {
            var oResult = await _roleService.Delete(id, CurrentUserId);
            
            return Ok(oResult);
        }
        #endregion

        #region UTILITY METHODS
        private async Task<(RoleDTO,List<PermissionDTO>)> GetRoleDTOWithPermissionDTOListByRoleId(Guid id)
        {
            List<PermissionDTO> lstPermissionDTO = new();

            var oRoleWithPermissions = await _roleService.GetByIdWithPermissions(id);

            var oRoleDTO = _mapper.Map<RoleDTO>(oRoleWithPermissions);

            oRoleWithPermissions.Permissions.ToList().ForEach(permission =>
            {
                lstPermissionDTO.Add(_mapper.Map<PermissionDTO>(permission));
            });

            return (oRoleDTO, lstPermissionDTO);
        }

        private List<RoleDTO> GetRoleDTOList()
        {
            var lstRoles = _roleService.GetAll();

            List<RoleDTO> lstRoleDTO = new();

            lstRoles.ForEach(role =>
            {
                lstRoleDTO.Add(_mapper.Map<RoleDTO>(role));
            });

            return lstRoleDTO;
        }

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
