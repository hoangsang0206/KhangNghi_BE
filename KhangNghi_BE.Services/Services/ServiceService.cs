using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Service?> GetByIdAsync(string id)
        {
            return await _context.Services
                .Include(x => x.ServiceImages)
                .FirstOrDefaultAsync(x => x.ServiceId == id);
        }
    }
}
