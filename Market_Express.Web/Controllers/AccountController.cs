using Market_Express.Application.DTOs.Access;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            if (returnUrl != null)
                ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpGet]
        public IActionResult SignIn(LoginRequestDTO model, string returnUrl)
        {
            /*
            var result = await _service.TryAuthenticateAsync(credential, password);

            if (result.Success)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Data.FullName),
                    new Claim(ClaimTypes.Email,result.Data.Email),
                    new Claim(ClaimTypes.MobilePhone, result.Data.PhoneNumber.ToString()),
                    new Claim("ImageName", result.Data.ImageName),
                    new Claim(ClaimTypes.Role, result.Data.AppUserRole.ToString()),
                    new Claim(ClaimTypes.Role, result.Data.EmployeeRole.ToString()),
                    new Claim("Token", result.Data.Token)
                };

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
            */
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
