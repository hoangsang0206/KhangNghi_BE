using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface IAuthorizationService
    {
        Task<bool> CheckPermissionAsync(string userId, string funcCode);
        Task<IEnumerable<Function>> GetAuthorizedFunctionsAsync(string groudId);
    }
}
