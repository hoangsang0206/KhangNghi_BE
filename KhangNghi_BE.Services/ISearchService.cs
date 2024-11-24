using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface ISearchService
    {
        Task<PagedList<SearchResultVM>> SearchServicesAndProductsAsync(string query, string? sortBy, int page, int pageSize);
    }
}
