using KhangNghi_BE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class ServiceCalalogService : IServiceCatalogService
    {
        private readonly KhangNghiContext _context;

        public ServiceCalalogService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<ServiceCatalog>> GetCatalogsAsync()
        {
            return await _context.ServiceCatalogs.ToListAsync();
        }
    }
}
