using Market_Express.Domain.Abstractions.DomainServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Market_Express.Web.Filters
{
    public class CheckPasswordFilter : IAsyncActionFilter
    {
        private readonly IAccountService _accountService;

        public CheckPasswordFilter(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                Guid userId = new(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (!await _accountService.HasValidPassword(userId))
                {
                    Debug.WriteLine("El usuario no tiene contraseña válida xd");
                }
            }

            //context.HttpContext.Response.Redirect("/Home/Index");

            await next();
        }
    }
}
