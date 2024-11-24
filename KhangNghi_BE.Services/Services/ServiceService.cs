using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace KhangNghi_BE.Services.Services
{
    public class ServiceService : IServiceService
    {
        private readonly KhangNghiContext _context;

        public ServiceService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<Service>> GetAsync(string? sortBy)
        {
            return await _context.Services
                .SortBy(sortBy)
                .ToListAsync();
        }

        public async Task<PagedList<Service>> GetAsync(string? sortBy, int page, int pageSize)
        {
            return await _context.Services
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<PagedList<Service>> SearchByNameAsync(string query, int page, int pageSize)
        {
            string[] keywords = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return await _context.Services
                .Where(p => keywords.All(k => p.ServiceName.Contains(k)))
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<Service?> GetByIdAsync(string id)
        {
            return await _context.Services
                .Include(x => x.ServiceImages)
                .FirstOrDefaultAsync(x => x.ServiceId == id);
        }

        public async Task<bool> CreateAsync(ServiceVM service)
        {
            Service _service = new Service
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                ShortDescription = service.ShortDescription,
                Description = service.Description,
                Price = service.Price,
                CalculationUnit = service.CalculationUnit,
                CatalogId = service.CatalogId,

                ServiceImages = new List<ServiceImage>
                {
                    new ServiceImage
                    {
                        ImageUrl = service.MainImage
                    }
                }
            };

            await _context.Services.AddAsync(_service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ServiceVM service)
        {
            Service? _service = await GetByIdAsync(service.ServiceId);

            if (_service == null)
            {
                return false;
            }

            _service.ServiceName = service.ServiceName;
            _service.ShortDescription = service.ShortDescription;
            _service.Description = service.Description;
            _service.Price = service.Price;
            _service.CalculationUnit = service.CalculationUnit;
            _service.CatalogId = service.CatalogId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Service? _service = await GetByIdAsync(id);

            if (_service == null)
            {
                return false;
            }

            _context.Services.Remove(_service);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
