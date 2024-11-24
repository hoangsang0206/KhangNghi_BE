using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KhangNghi_BE.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly KhangNghiContext _context;

        public ProductService(KhangNghiContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products
                .IncludeAllForeign()
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetAsync(string? sortBy)
        {
            return await _context.Products
                .IncludePromotions()
                .SortBy(sortBy)
                .ToListAsync();
        }

        public Task<PagedList<Product>> GetAsync(string? sortBy, int page, int pageSize)
        {
            return _context.Products
                .IncludePromotions()
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<PagedList<Product>> GetByCatalogAsync(string? sortBy, string catalogId, int page, int pageSize)
        {
            return _context.Products
                .IncludePromotions()
                .Where(p => p.Catalogs.Any(c => c.CatalogId == catalogId))
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<PagedList<Product>> GetByNameAsync(string? sortBy, string search, int page, int pageSize)
        {
            string[] keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _context.Products
                .IncludePromotions()
                .Where(p => keywords.All(k => p.ProductName.Contains(k)))
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<bool> CreateAsync(ProductVM product, List<string> imageUrls)
        {
            Product _product = new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                CalculationUnit = product.CalculationUnit,
                IsActive = false,
                Origin = product.Origin,
                Warranty = product.Warranty
            };

            if(product.CatalogIds != null)
            {
                foreach (string catalogId in product.CatalogIds)
                {
                    ProductCatalog? catalog = await _context.ProductCatalogs
                        .FirstOrDefaultAsync(c => c.CatalogId == catalogId);

                    if (catalog == null)
                    {
                        continue;
                    }

                    _product.Catalogs.Add(catalog);
                }
            }

            foreach (string image in imageUrls)
            {
                _product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = image
                });
            }

            foreach (var spec in product.Specs)
            {
                _product.ProductSpecifications.Add(new ProductSpecification
                {
                    SpecName = spec.SpecName,
                    SpecValue = spec.SpecValue
                });
            }

            await _context.Products.AddAsync(_product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ProductVM product)
        {
            Product? _product = await _context.Products
                .Include(p => p.Catalogs)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSpecifications)
                .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);


            if (_product == null)
            {
                return false;
            }

            _product.ProductName = product.ProductName;
            _product.ShortDescription = product.ShortDescription;
            _product.Description = product.Description;
            _product.OriginalPrice = product.OriginalPrice;
            _product.Price = product.Price;
            _product.CalculationUnit = product.CalculationUnit;
            _product.Origin = product.Origin;
            _product.Warranty = product.Warranty;

            _product.Catalogs.Clear();
            if (product.CatalogIds != null)
            {
                foreach (string catalogId in product.CatalogIds)
                {
                    ProductCatalog? catalog = await _context.ProductCatalogs
                        .FirstOrDefaultAsync(c => c.CatalogId == catalogId);

                    if(catalog == null)
                    {
                        continue;
                    }

                    _product.Catalogs.Add(catalog);
                }
            }

            //_product.ProductImages.Clear();
            //foreach (var image in product.Images)
            //{
            //    _product.ProductImages.Add(new ProductImage
            //    {
            //        ImageUrl = image.ImageUrl
            //    });
            //}

            _product.ProductSpecifications.Clear();
            foreach (var spec in product.Specs)
            {
                _product.ProductSpecifications.Add(new ProductSpecification
                {
                    SpecName = spec.SpecName,
                    SpecValue = spec.SpecValue
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Product? product = await _context.Products
                .Include(p => p.ContractDetails)
                .Include(p => p.ProductsInWarehouses)
                .Include(p => p.Promotions)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSpecifications)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null || product.ContractDetails.Any() 
                || product.ProductsInWarehouses.Any() || product.Promotions.Any())
            {
                return false;
            }

            _context.ProductImages.RemoveRange(product.ProductImages);
            _context.ProductSpecifications.RemoveRange(product.ProductSpecifications);
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
