using AutoMapper;
using Market_Express.Application.DTOs.Account;
using Market_Express.Application.DTOs.Address;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService,
                                 IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ProfileViewModel oViewModel = new();

            var oUser = await _accountService.GetUserInfo(CurrentUserId);
            var lstAddress = await _accountService.GetAddressList(CurrentUserId);

            oViewModel.AppUser = _mapper.Map<AppUserProfileDTO>(oUser);

            lstAddress?.ToList().ForEach(add =>
            {
                oViewModel.Addresses.Add(_mapper.Map<AddressDTO>(add));
            });

            return View(oViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (returnUrl != null)
                ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO oModel, string returnUrl)
        {
            var oUser = _mapper.Map<AppUser>(oModel);

            var oResult = _accountService.TryAuthenticate(ref oUser);

            if (oResult.Success)
            {
                var lstPermisos = await _accountService.GetPermissionList(oUser.Id);

                List<Claim> lstClaims = new();

                lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()));
                lstClaims.Add(new Claim(ClaimTypes.Name, oUser.Name));
                lstClaims.Add(new Claim("Alias", oUser.Alias));
                lstClaims.Add(new Claim("Identification", oUser.Identification));
                lstClaims.Add(new Claim(ClaimTypes.Email, oUser.Email));
                lstClaims.Add(new Claim(ClaimTypes.MobilePhone, oUser.Phone));
                lstClaims.Add(new Claim(ClaimTypes.Role, oUser.Type));

                lstPermisos.ForEach(per =>
                {
                    lstClaims.Add(new Claim(ClaimTypes.Role, per.PermissionCode));
                });

                var oIdentity = new ClaimsIdentity(lstClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var oPrincipal = new ClaimsPrincipal(oIdentity);

                await HttpContext.SignInAsync(oPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMonths(12),
                });

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["LoginMessage"] = oResult.Message;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> AddressManager()
        {
            var lstAddress = await _accountService.GetAddressList(CurrentUserId);

            List<AddressDTO> lstViewModel = new();

            lstAddress?.ToList().ForEach(add =>
            {
                lstViewModel.Add(_mapper.Map<AddressDTO>(add));
            });

            return PartialView("_AddressManagementPartial", lstViewModel);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressDTO model)
        {
            model.Id = null;

            var oResult = await _accountService.CreateAddress(CurrentUserId, _mapper.Map<Address>(model));

            return Ok(oResult);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressDTO model)
        {
            var oResult = await _accountService.EditAddress(_mapper.Map<Address>(model));

            return Ok(oResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressInfo([FromQuery(Name = "addressId")] Guid addressId)
        {
            var oAddress = await _accountService.GetAddressInfo(addressId);

            var oResult = new
            {
                Success = oAddress != null,
                Data = _mapper.Map<AddressDTO>(oAddress)
            };

            return Ok(oResult);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO model)
        {
            var oResult = await _accountService.TryChangePassword(CurrentUserId, model.CurrentPass, model.NewPass, model.NewPassConfirmation);

            if (oResult.Success)
                return Ok(oResult);
            else
                return BadRequest(oResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAlias()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("");

            var oUser = await _accountService.GetUserInfo(CurrentUserId);

            return Content(oUser.Alias);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAlias(string alias)
        {
            var oResult = await _accountService.TryChangeAlias(CurrentUserId, alias);

            if (oResult.Success)
                return Ok(oResult);
            else
                return BadRequest(oResult);
        }
        #endregion
    }
}