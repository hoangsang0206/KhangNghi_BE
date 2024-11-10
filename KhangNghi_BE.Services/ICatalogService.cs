using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductCatalog>> GetCatalogsAsync();
        Task<IEnumerable<ProductCatalog>> GetProductCatalogAsync(string productId);
        Task<ProductCatalog?> GetByIdAsync(string catalogId);

        Task<bool> CreateAsync(CatalogVM catalog);
        Task<bool> UpdateAsync(CatalogVM catalog);
        Task<bool> DeleteAsync(string catalogId);
    }
}
