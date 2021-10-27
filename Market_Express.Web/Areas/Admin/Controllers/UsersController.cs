using AutoMapper;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Domain.Abstractions.DomainServices;
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
    [Authorize(Roles = "USE_MAN_GEN")]
    public class UsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public UsersController(IAppUserService appUserService,
                               IMapper mapper)
        {
            _appUserService = appUserService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lstAppUserDTO = GetAppUserDTOList();

            return View(lstAppUserDTO);
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
        #endregion
    }
}
