using System;
using Auriqo_Web_Api_Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auriqo_Web_Api_Backend.Middlewares;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Cookies["Auriqo-Authorization-Token"];

        if (token == null)
        {
            context.Result = new JsonResult(new { message = " SESSION EXPIRED. KINDLY LOGIN AGAIN" }, new
            {
                StatusCodes = 401
            });
            return;
        }

        var tokenService = context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService;

        var userId = tokenService?.VerifyTokenAndGetId(token);

        if (userId == Guid.Empty)
        {
            context.Result = new JsonResult(new { message = "FORBIDDEN TO ACCESS THE PAGES. KINDLY LOGIN AGAIN" }, new
            {
                statusCodes = 403
            });
            return;
        }

        context.HttpContext.Items["userId"] = userId;
    }
}
