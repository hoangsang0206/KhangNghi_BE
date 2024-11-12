using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAsync(string? sortBy);
        Task<PagedList<Service>> GetAsync(string? sortBy, int page, int pageSize);
        Task<Service?> GetByIdAsync(string id);

        Task<bool> CreateAsync(ServiceVM service);
        Task<bool> UpdateAsync(ServiceVM service);
        Task<bool> DeleteAsync(string id);
    }
}
