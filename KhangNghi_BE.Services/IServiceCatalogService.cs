using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IServiceCatalogService
    {
        Task<IEnumerable<ServiceCatalog>> GetAllAsync();
        Task<ServiceCatalog?> GetByIdAsync(string id);
        Task<bool> CreateAsync(CatalogVM catalog);
        Task<bool> UpdateAsync(CatalogVM catalog);
        Task<bool> DeleteAsync(string id);
    }
}
