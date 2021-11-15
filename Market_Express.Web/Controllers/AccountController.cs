using AutoMapper;
using Market_Express.Application.DTOs.Account;
using Market_Express.Application.DTOs.Address;
using Market_Express.Application.DTOs.AppUser;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
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
        private readonly IBinnacleAccessService _binnacleAccessService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService,
                                 IBinnacleAccessService binnacleAccessService,
                                 IAddressService addressService,
                                 IMapper mapper)
        {
            _accountService = accountService;
            _binnacleAccessService = binnacleAccessService;
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ProfileViewModel oViewModel = new();

            var oUser = await _accountService.GetUserInfo(CurrentUserId);
            var lstAddress = await _addressService.GetAllByUserId(CurrentUserId);

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
            if (IsAuthenticated)
                return Redirect("/");

            if (returnUrl != null)
                ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model, string returnUrl)
        {
            AppUser oUser = new(model.Email, model.Password);

            var oResult = _accountService.TryAuthenticate(ref oUser);

            if (!oResult.Success)
            {
                ViewData["LoginMessage"] = oResult.Message;

                return View();
            }

            List<Claim> lstClaims = new();

            if (oUser.Type == AppUserType.ADMINISTRADOR)
            {
                var lstPermisos = await _accountService.GetPermissionList(oUser.Id);

                lstPermisos.ForEach(per =>
                {
                    lstClaims.Add(new Claim(ClaimTypes.Role, per.PermissionCode));
                });
            }

            lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()));
            lstClaims.Add(new Claim(ClaimTypes.Name, oUser.Name));
            lstClaims.Add(new Claim("Identification", oUser.Identification));
            lstClaims.Add(new Claim(ClaimTypes.Email, oUser.Email));
            lstClaims.Add(new Claim(ClaimTypes.MobilePhone, oUser.Phone));
            lstClaims.Add(new Claim(ClaimTypes.Role, oUser.Type.ToString()));

            var oIdentity = new ClaimsIdentity(lstClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var oPrincipal = new ClaimsPrincipal(oIdentity);

            await HttpContext.SignInAsync(oPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.Now.AddHours(12),
            });

            await _binnacleAccessService.RegisterAccess(oUser.Id);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!IsAuthenticated)
                return Redirect("/");

            await _binnacleAccessService.RegisterExit(CurrentUserId);

            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressTable()
        {
            var lstAddress = await _addressService.GetAllByUserId(CurrentUserId);

            List<AddressDTO> lstViewModel = new();

            lstAddress?.ToList().ForEach(add =>
            {
                lstViewModel.Add(_mapper.Map<AddressDTO>(add));
            });

            return PartialView("_AddressManagementPartial", lstViewModel);
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> SetAddressToUse(Guid addressId)
        {
            var oResult = await _addressService.SetForUse(addressId, CurrentUserId);
            
            return Ok(oResult);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(AddressDTO model)
        {
            Address oAddress = new(model.Name, model.Detail);

            var oResult = await _addressService.Create(oAddress, CurrentUserId);

            return Ok(oResult);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressDTO model)
        {
            Address oAddress = new(model.Id, model.Name, model.Detail, model.InUse);

            var oResult = await _addressService.Edit(oAddress, CurrentUserId);

            return Ok(oResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressInfo([FromQuery(Name = "addressId")] Guid addressId)
        {
            var oAddress = await _addressService.GetById(addressId);

            var oResult = new
            {
                Success = oAddress != null,
                Data = _mapper.Map<AddressDTO>(oAddress)
            };

            return Ok(oResult);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO model)
        {
            var oResult = await _accountService.ChangePassword(CurrentUserId, model.CurrentPass, model.NewPass, model.NewPassConfirmation, model.IsFirstLogin);

            if (model.IsFirstLogin)
            {
                if (oResult.Success)
                    return RedirectToAction("Index", "Home");

                ViewData["MessageResult"] = oResult.Message;

                return View();
            }

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