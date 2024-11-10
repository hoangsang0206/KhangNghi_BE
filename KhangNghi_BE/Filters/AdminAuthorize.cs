using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace KhangNghi_BE.Filters
{
    public class AdminAuthorize : Attribute, IAuthorizationFilter
    {
        public string Code { get; set; } = "";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string? userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            IAuthorizationService _authorizeService = context.HttpContext.RequestServices
                .GetRequiredService<IAuthorizationService>();

            if(!_authorizeService.CheckPermissionAsync(userId, Code).Result)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
