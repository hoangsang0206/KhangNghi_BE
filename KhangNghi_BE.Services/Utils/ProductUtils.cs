using KhangNghi_BE.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Utils
{
    public static class ProductUtils
    {
        public static IQueryable<Product> SortBy(this IQueryable<Product> products, string? sortBy)
        {
            if (sortBy == null)
            {
                return products;
            }

            return sortBy switch
            {
                "name-az" => products.OrderBy(p => p.ProductName),
                "name-za" => products.OrderByDescending(p => p.ProductName),
                "price-asc" => products.OrderBy(p => p.Price),
                "price-desc" => products.OrderByDescending(p => p.Price),
                "random" => products.OrderBy(p => Guid.NewGuid()),
                _ => products
            };
        }

        public static IQueryable<Product> IncludeAllForeign(this IQueryable<Product> products)
        {
            products = products
                .Include(p => p.ProductImages)
                .Include(p => p.Catalogs)
                .Include(p => p.ProductsInWarehouses)
                .Include(p => p.ProductSpecifications)
                .Include(p => p.Promotions);

            foreach (Product product in products)
            {
                product.Promotions = product.Promotions
                    .Where(p => p.IsActive == true)
                    .ToList();
            }

            return products;
        }

        public static IQueryable<Product> IncludePromotions(this IQueryable<Product> products)
        {
            products = products
                .Include(p => p.ProductImages)
                .Include(p => p.Promotions);

            foreach (Product product in products)
            {
                product.Promotions = product.Promotions
                    .Where(p => p.IsActive == true)
                    .ToList();
            }

            return products;
        }
    }
}
