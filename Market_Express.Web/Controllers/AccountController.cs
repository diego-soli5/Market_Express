using AutoMapper;
using Market_Express.Application.DTOs.Access;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService,
                                 IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            ProfileViewModel oViewModel = new();
            oViewModel.Name = User.FindFirstValue(ClaimTypes.Name);
            oViewModel.Identification = User.FindFirstValue("Identification");
            oViewModel.Email = User.FindFirstValue(ClaimTypes.Email);
            oViewModel.Phone = User.FindFirstValue(ClaimTypes.MobilePhone);

            return View(oViewModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (returnUrl != null)
                ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO oModel, string returnUrl)
        {
            var oUser = _mapper.Map<AppUser>(oModel);

            var oResult = _accountService.TryAuthenticate(ref oUser);

            if (oResult.Success) 
            {
                var lstPermisos = await _accountService.GetPermisos(oUser.Id);

                List<Claim> lstClaims = new();

                lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()));
                lstClaims.Add(new Claim(ClaimTypes.Name, oUser.Name));
                lstClaims.Add(new Claim("Identification", oUser.Identification));
                lstClaims.Add(new Claim(ClaimTypes.Email, oUser.Email));
                lstClaims.Add(new Claim(ClaimTypes.MobilePhone, oUser.Phone));
                lstClaims.Add(new Claim(ClaimTypes.Role, oUser.Type));

                lstPermisos.ForEach(per =>
                {
                    lstClaims.Add(new Claim(ClaimTypes.Role, per.Name));
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

            return RedirectToAction("Index", "Home");
        }
    }
}
