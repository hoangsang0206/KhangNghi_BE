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
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    OriginalPrice = p.OriginalPrice,
                    Price = p.Price,
                    ShortDescription = p.ShortDescription,
                    Description = p.Description,
                    CalculationUnit = p.CalculationUnit,
                    Origin = p.Origin,
                    Warranty = p.Warranty,
                    IsActive = p.IsActive,
                    ProductImages = p.ProductImages
                        .Select(i => new ProductImage
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl
                        }).ToList(),
                    Catalogs = p.Catalogs.Select(c => new ProductCatalog
                    {
                        CatalogId = c.CatalogId,
                        CatalogName = c.CatalogName
                    }).ToList(),
                    ProductsInWarehouses = p.ProductsInWarehouses
                        .Select(w => new ProductsInWarehouse
                        {
                            WarehouseId = w.WarehouseId,
                            Quantity = w.Quantity
                        }).ToList(),
                    ProductSpecifications = p.ProductSpecifications
                        .Select(s => new ProductSpecification
                        {
                            Id = s.Id,
                            SpecName = s.SpecName,
                            SpecValue = s.SpecValue
                        }).ToList(),
                    Promotions = p.Promotions.Where(p => p.IsActive == true)
                        .Select(p => new Promotion
                        {
                            PromotionId = p.PromotionId,
                            PromotionName = p.PromotionName,
                            PromotionType = p.PromotionType,
                            DiscountAmount = p.DiscountAmount,
                            MaxDiscountAmount = p.MaxDiscountAmount,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            IsActive = p.IsActive
                        }).ToList()
                        .ToList()
                });

            return products;
        }

        public static IQueryable<Product> IncludePromotions(this IQueryable<Product> products)
        {
            products = products
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    OriginalPrice = p.OriginalPrice,
                    Price = p.Price,
                    ShortDescription = p.ShortDescription,
                    Description = p.Description,
                    CalculationUnit = p.CalculationUnit,
                    Origin = p.Origin,
                    Warranty = p.Warranty,
                    IsActive = p.IsActive,
                    ProductImages = p.ProductImages
                        .Select(i => new ProductImage
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl
                        }).ToList(),
                    Catalogs = p.Catalogs
                        .Select(c => new ProductCatalog
                        {
                            CatalogId = c.CatalogId,
                            CatalogName = c.CatalogName
                        }).ToList(),
                    Promotions = p.Promotions.Where(p => p.IsActive == true)
                        .Select(p => new Promotion
                        {
                            PromotionId = p.PromotionId,
                            PromotionName = p.PromotionName,
                            PromotionType = p.PromotionType,
                            DiscountAmount = p.DiscountAmount,
                            MaxDiscountAmount = p.MaxDiscountAmount,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            IsActive = p.IsActive
                        }).ToList()
                        .ToList()
                });

            return products;
        }
    }
}
