using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class ContractUtils
    {
        public static IQueryable<Contract> SortBy(this IQueryable<Contract> contracts, string? value)
        {
            switch (value)
            {
                case "newest":
                    return contracts.OrderBy(c => c.CreateAt);
                case "oldest":
                    return contracts.OrderByDescending(c => c.CreateAt);
                default:
                    return contracts;
            }
        }

        public static IQueryable<Contract> FilterBy(this IQueryable<Contract> contracts, string? value)
        {
            switch (value)
            {
                case "incomplete":
                    return contracts.Where(c => c.CompletedAt == null);
                case "completed":
                    return contracts.Where(c => c.CompletedAt != null);
                default:
                    return contracts;
            }
        }
    }
}
