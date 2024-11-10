using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAsync(string? sortBy);
        Task<PagedList<Product>> GetAsync(string? sortBy, int page, int pageSize);
        Task<PagedList<Product>> GetByCatalogAsync(string? sortBy, string catalogId, int page, int pageSize);
        Task<PagedList<Product>> GetByNameAsync(string? sortBy, string search, int page, int pageSize);
        Task<Product?> GetByIdAsync(string id);

        Task<bool> CreateAsync(ProductVM product);
        Task<bool> UpdateAsync(ProductVM product);
        Task<bool> DeleteAsync(string id);
    }
}
