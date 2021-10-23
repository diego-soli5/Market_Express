using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BinnacleController : Controller
    {
        public BinnacleController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
