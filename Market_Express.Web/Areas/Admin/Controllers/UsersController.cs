using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "USE_MAN_GEN")]
    public class UsersController : Controller
    {
        public UsersController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
