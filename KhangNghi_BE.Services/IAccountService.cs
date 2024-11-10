using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IAccountService
    {
        Task<User?> LoginAsync(Login login);
        Task<User?> RegisterAsync(Register register);
        Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
        Task<RefreshToken?> CreateRefreshTokenAsync(string userId, string token, string jwtId);
        Task<bool> UpdateRefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshTokens(string userId);
        Task<User?> GetUserAsync(string userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(string userId, ChangePassword password);
    }
}
