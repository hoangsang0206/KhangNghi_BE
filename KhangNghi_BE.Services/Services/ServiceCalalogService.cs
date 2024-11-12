using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class ServiceCalalogService : IServiceCatalogService
    {
        private readonly KhangNghiContext _context;

        public ServiceCalalogService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<ServiceCatalog>> GetAllAsync()
        {
            return await _context.ServiceCatalogs.ToListAsync();
        }

        public async Task<ServiceCatalog?> GetByIdAsync(string id)
        {
            return await _context.ServiceCatalogs
                .FirstOrDefaultAsync(x => x.CatalogId == id);
        }

        public async Task<bool> CreateAsync(CatalogVM catalog)
        {
            ServiceCatalog newCatalog = new ServiceCatalog
            {
                CatalogId = catalog.CatalogId,
                CatalogName = catalog.CatalogName,
            };

            await _context.ServiceCatalogs.AddAsync(newCatalog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(CatalogVM catalog)
        {
            ServiceCatalog? oldCatalog = await _context.ServiceCatalogs
                .FirstOrDefaultAsync(x => x.CatalogId == catalog.CatalogId);

            if (oldCatalog == null)
            {
                return false;
            }

            oldCatalog.CatalogName = catalog.CatalogName;

            _context.ServiceCatalogs.Update(oldCatalog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            ServiceCatalog? catalog = await _context.ServiceCatalogs
                .FirstOrDefaultAsync(x => x.CatalogId == id);

            if (catalog == null)
            {
                return false;
            }

            _context.ServiceCatalogs.Remove(catalog);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
