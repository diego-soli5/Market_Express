using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.CrossCutting.Logging;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Market_Express.Web.Controllers
{
    public class ErrorController : BaseController
    {
        private readonly IBusisnessMailService _mailService;

        public ErrorController(IBusisnessMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Handle()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;

            if (exception is NotFoundException)
            {
                var notFoundException = exception as NotFoundException;

                return View("NotFound", new NotFoundViewModel(notFoundException.ResourceId, notFoundException.ResourceType));
            }

            if (exception is UnauthorizedException)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            Task.Run(() =>
            {
                ExceptionLogger.LogException(exception);
                NotifiError(exception);
            });

            return View("Error", new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier,exception.Message));
        }

        private void NotifiError(Exception exeption)
        {
            string sSubject = "Ocurrió un error en Market Express";
            string sBody = $"{exeption.Message}\n\n{exeption?.StackTrace}";
            string sDest = "1diego321@gmail.com";

            _mailService.SendMail(sSubject, sBody, sDest);
        }
    }
}
