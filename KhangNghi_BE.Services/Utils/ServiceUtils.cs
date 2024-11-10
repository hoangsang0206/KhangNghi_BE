using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class ServiceUtils
    {
        public static IQueryable<Service> SortBy(this IQueryable<Service> services, string? sortBy)
        {
            if (sortBy == null)
            {
                return services;
            }

            return sortBy switch
            {
                "name-az" => services.OrderBy(s => s.ServiceName),
                "name-za" => services.OrderByDescending(s => s.ServiceName),
                "price-asc" => services.OrderBy(s => s.Price),
                "price-desc" => services.OrderByDescending(s => s.Price),
                "random" => services.OrderBy(s => Guid.NewGuid()),
                _ => services
            };
        }
    }
}
