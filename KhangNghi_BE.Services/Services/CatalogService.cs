using KhangNghi_BE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly KhangNghiContext _context;

        public CatalogService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<ProductCatalog>> GetCatalogsAsync()
        {
            return await _context.ProductCatalogs.ToListAsync();
        }

        public async Task<IEnumerable<ProductCatalog>> GetProductCatalogAsync(string productId)
        {
            return await _context.ProductCatalogs
                .Where(c => c.Products.Any(p => p.ProductId == productId))
                .ToListAsync();
        }
    }
}
