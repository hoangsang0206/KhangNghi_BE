using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface IServiceCatalogService
    {
        Task<IEnumerable<ServiceCatalog>> GetCatalogsAsync();
    }
}
