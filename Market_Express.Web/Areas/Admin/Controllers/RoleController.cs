using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Market_Express.Domain.Abstractions.DomainServices;
using AutoMapper;
using System.Collections.Generic;
using Market_Express.Application.DTOs.Role;
using System;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "ROL_MAN_GEN")]
    public class RoleController : Controller
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

        [HttpGet("{id}")]
        public IActionResult Edit(Guid id)
        {
            return View();
        }
    }
}
