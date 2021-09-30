using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    public class CartController : Controller
    {
        public IActionResult GetCartArticlesCount()
        {
            if (!User.Identity.IsAuthenticated)
                return Content("0");

            return Content("19");
        }
    }
}
