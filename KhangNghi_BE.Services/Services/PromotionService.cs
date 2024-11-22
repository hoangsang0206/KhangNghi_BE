using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly KhangNghiContext _context;

        public PromotionService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<Promotion>> GetAsync(string[] idList)
        {
            return await _context.Promotions
                .Where(p => idList.Contains(p.PromotionId))
                .ToListAsync();
        }

        public async Task<PagedList<Promotion>> GetAsync(string? filterBy, string? sortBy, int page, int pageSize)
        {
            return await _context.Promotions
                .SortBy(sortBy)
                .FilterBy(filterBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<PagedList<Promotion>> GetActivePromotionsAsync(string? sortBy, int page, int pageSize)
        {
            return _context.Promotions
                .Where(p => p.IsActive == true && p.StartDate <= DateTime.Now && p.EndDate > DateTime.Now)
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<Promotion?> GetByIdAsync(string id)
        {
            return await _context.Promotions
                .Where(p => p.PromotionId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIsActive(string id)
        {
            Promotion? promotion = await GetByIdAsync(id);

            if (promotion == null)
            {
                return false;
            }

            return promotion.IsActive == true
                && promotion.StartDate <= DateTime.Now
                && promotion.EndDate > DateTime.Now;
        }

        public Task<bool> CreateAsync(PromotionVM promotion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(PromotionVM promotion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
