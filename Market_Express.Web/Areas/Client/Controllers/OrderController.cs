using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
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

        [HttpGet]
        [Route("/Client/Order/Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            return View();
        }
    }
}
