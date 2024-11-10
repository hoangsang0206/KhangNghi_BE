namespace KhangNghi_BE.Services
{
    public interface IAuthorizationService
    {
        Task<bool> CheckPermissionAsync(string userId, string funcCode);
    }
}
