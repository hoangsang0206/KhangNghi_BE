using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace KhangNghi_BE.Filters
{
    public class AdminAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizeService;

        public string Code { get; set; } = "";

        public AdminAuthorize(IAuthorizationService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
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

            if(!await _authorizeService.CheckPermissionAsync(userId, Code))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
