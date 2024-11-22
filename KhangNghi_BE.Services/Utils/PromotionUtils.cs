using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class PromotionUtils
    {
        public static IQueryable<Promotion> SortBy(this IQueryable<Promotion> promotions, string? value)
        {
            switch (value)
            {
                case "start-date-asc":
                    return promotions.OrderBy(p => p.StartDate);
                case "start-date-desc":
                    return promotions.OrderByDescending(p => p.StartDate);
                case "end-date-asc":
                    return promotions.OrderBy(p => p.EndDate);
                case "end-date-desc":
                    return promotions.OrderByDescending(p => p.EndDate);
                default:
                    return promotions;
            }
        }

        public static IQueryable<Promotion> FilterBy(this IQueryable<Promotion> promotions, string? value)
        {
            switch (value)
            {
                case "active":
                    return promotions.Where(p => p.IsActive == true 
                                                && p.StartDate <= DateTime.Now 
                                                && p.EndDate > DateTime.Now);
                case "ended":
                    return promotions.Where(p => p.EndDate <= DateTime.Now);

                case "upcoming":
                    return promotions.Where(p => p.StartDate > DateTime.Now);
                default:
                    return promotions;
            }
        }
    }
}
