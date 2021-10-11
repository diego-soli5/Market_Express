using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Market_Express.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public IActionResult Handle()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;

            if(exception is NotFoundException)
            {
                var notFoundException = exception as NotFoundException;

                return View("NotFound", new NotFoundViewModel(notFoundException.ResourceId,notFoundException.ResourceType));
            }

            return View("Error",new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }
    }
}
