using AutoMapper;
using Market_Express.Application.DTOs.Access;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult SignIn(string sReturnUrl)
        {
            if (sReturnUrl != null)
                ViewData["returnUrl"] = sReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginRequestDTO oModel, string sReturnUrl)
        {
            var oUser = _mapper.Map<Usuario>(oModel);

            var oResult = _accountService.TryAuthenticate(ref oUser);

            if (oResult.Success) 
            {
                var lstPermisos = await _accountService.GetPermisos(oUser.Id);

                List<Claim> lstClaims = new();

                lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()));
                lstClaims.Add(new Claim(ClaimTypes.Name, oUser.Nombre));
                lstClaims.Add(new Claim(ClaimTypes.Email, oUser.Email));
                lstClaims.Add(new Claim(ClaimTypes.MobilePhone, oUser.Telefono));
                lstClaims.Add(new Claim(ClaimTypes.Role, oUser.Tipo));

                lstPermisos.ForEach(per =>
                {
                    lstClaims.Add(new Claim(ClaimTypes.Role, per.Nombre));
                });

                var oIdentity = new ClaimsIdentity(lstClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var oPrincipal = new ClaimsPrincipal(oIdentity);

                await HttpContext.SignInAsync(oPrincipal);

                if (!string.IsNullOrEmpty(sReturnUrl))
                {
                    return LocalRedirect(sReturnUrl);
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
