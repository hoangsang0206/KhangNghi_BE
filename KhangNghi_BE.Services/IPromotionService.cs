using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> GetAsync(string[] idList);
        Task<PagedList<Promotion>> GetAsync(string? filterBy, string? sortBy, int page, int pageSize);
        Task<PagedList<Promotion>> GetActivePromotionsAsync(string? sortBy, int page, int pageSize);
        Task<Promotion?> GetByIdAsync(string id);
        Task<bool> CheckIsActive(string id);

        Task<bool> CreateAsync(PromotionVM promotion);
        Task<bool> UpdateAsync(PromotionVM promotion);
        Task<bool> DeleteAsync(string id);
    }
}
