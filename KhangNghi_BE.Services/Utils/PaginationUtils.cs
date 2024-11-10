using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Utils
{
    public static class PaginationUtils
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int page, int itemsPerPage)
        {
            if (page <= 0)
                page = 1;
            if (itemsPerPage <= 0)
                itemsPerPage = 1;

            int remaingItems = superset.Count() - (page * itemsPerPage);

            return new PagedList<T>
            {
                Items = await superset.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(superset.Count() / (double)itemsPerPage),
                PageSize = itemsPerPage,
                TotalItems = superset.Count(),
                RemaingItems = remaingItems > 0 ? remaingItems : 0
            };
        }
    }
}
