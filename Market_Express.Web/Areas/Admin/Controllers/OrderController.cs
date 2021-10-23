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
    [Authorize(Roles = "ORD_MAN_GEN")]
    public class OrderController : Controller
    {
        public OrderController()
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
