using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface IAccountService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<User?> RegisterAsync(string username, string password);
        Task<bool> ChangePasswordAsync(string userId, string newPassword);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task<RefreshToken> CreateRefreshTokenAsync(string userId);
        Task<User> GetUserAsync(string userId);
    }
}
