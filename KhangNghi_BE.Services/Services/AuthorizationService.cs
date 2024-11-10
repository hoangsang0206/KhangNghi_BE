using KhangNghi_BE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly KhangNghiContext _context;

        public AuthorizationService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckPermissionAsync(string userId, string funcCode)
        {
            User? user = await _context.Users
                .Include(u => u.Group)
                .ThenInclude(g => g.FunctionAuthorizations)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            bool isAuthorized = user.Group?.FunctionAuthorizations
                .Any(fa => fa.FunctionId == funcCode && fa.IsAuthorized == true) 
                ?? false;
        }
    }
}
