using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductCatalog>> GetCatalogsAsync();
        Task<IEnumerable<ProductCatalog>> GetProductCatalogAsync(string productId);
    }
}
