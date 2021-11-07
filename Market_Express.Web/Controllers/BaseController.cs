using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Market_Express.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Guid CurrentUserId => new (User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected bool IsAuthenticated => User.Identity.IsAuthenticated;
    }
}
