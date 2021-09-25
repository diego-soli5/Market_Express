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
        public IActionResult SignIn(string returnUrl)
        {
            if (returnUrl != null)
                ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginRequestDTO model, string returnUrl)
        {
            var oUser = _mapper.Map<Usuario>(model);

            var result = _accountService.TryAuthenticate(oUser);

            if (result.Success)
            {
                var permisos = await _accountService.GetPermisos(oUser.Id);

                List<Claim> claims = new();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, oUser.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, oUser.Nombre));
                claims.Add(new Claim(ClaimTypes.Email, oUser.Email));
                claims.Add(new Claim(ClaimTypes.MobilePhone, oUser.Telefono));
                claims.Add(new Claim(ClaimTypes.Role, oUser.Tipo));

                permisos.ForEach(per =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, per.Nombre));
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["LoginMessage"] = result.Message;

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
