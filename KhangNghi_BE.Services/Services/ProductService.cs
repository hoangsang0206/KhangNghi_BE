using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly KhangNghiContext _context;

        public ProductService(KhangNghiContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products
                .IncludeAllForeign()
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetAsync(string? sortBy)
        {
            return await _context.Products
                .IncludePromotions()
                .Include(p => p.ProductImages)
                .SortBy(sortBy)
                .ToListAsync();
        }

        public Task<PagedList<Product>> GetAsync(string? sortBy, int page, int pageSize)
        {
            return _context.Products
                .IncludePromotions()
                .Include(p => p.ProductImages)
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<PagedList<Product>> GetByCatalogAsync(string? sortBy, string catalogId, int page, int pageSize)
        {
            return _context.Products
                .IncludePromotions()
                .Include(p => p.ProductImages)
                .Where(p => p.Catalogs.Any(c => c.CatalogId == catalogId))
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<PagedList<Product>> GetByNameAsync(string? sortBy, string search, int page, int pageSize)
        {
            string[] keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _context.Products
                .IncludePromotions()
                .Include(p => p.ProductImages)
                .Where(p => keywords.All(k => p.ProductName.Contains(k)))
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }
    }
}
