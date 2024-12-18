﻿using KhangNghi_BE.Data.Models;
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
                .AsNoTracking()
                .Include(u => u.Group)
                .ThenInclude(g => g.FunctionAuthorizations)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            if(user.Group?.HasAllPermission == true)
            {
                return true;
            }

            bool isAuthorized = user.Group?.FunctionAuthorizations
                .Any(fa => fa.FunctionId == funcCode && fa.IsAuthorized == true) 
                ?? false;

            return isAuthorized;
        }

        public async Task<IEnumerable<Function>> GetAuthorizedFunctionsAsync(string groudId)
        {
            IEnumerable<FunctionAuthorization> authorizations = await _context.FunctionAuthorizations
                .Where(fa => fa.GroupId == groudId && fa.IsAuthorized == true)
                .Select(fa => new FunctionAuthorization
                {
                    AuthId = fa.AuthId,
                    GroupId = fa.GroupId,
                    FunctionId = fa.FunctionId,
                    Function = fa.Function,
                })
                .ToListAsync();

            return authorizations.Select(fa => fa.Function).ToList();
        }
    }
}
