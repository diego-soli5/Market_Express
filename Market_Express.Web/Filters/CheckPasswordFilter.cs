using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Web.Controllers;
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
                string requestUrl = context.HttpContext.Request.Path.Value.ToString();

                if (!await _accountService.HasValidPassword(userId))
                {
                    if (requestUrl != "/Account/ChangePassword")
                    {
                        context.HttpContext.Response.Redirect("/Account/ChangePassword");
                    }
                }
                else
                {
                    if (requestUrl == "/Account/ChangePassword")
                    {
                        context.HttpContext.Response.Redirect("/Home/Index");
                    }
                }
            }

            await next();
        }
    }
}
