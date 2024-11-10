﻿using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly KhangNghiContext _context;

        public CatalogService(KhangNghiContext context) => _context = context;

        public async Task<IEnumerable<ProductCatalog>> GetCatalogsAsync()
        {
            return await _context.ProductCatalogs.ToListAsync();
        }

        public async Task<IEnumerable<ProductCatalog>> GetProductCatalogAsync(string productId)
        {
            return await _context.ProductCatalogs
                .Where(c => c.Products.Any(p => p.ProductId == productId))
                .ToListAsync();
        }

        public async Task<ProductCatalog?> GetByIdAsync(string catalogId)
        {
            return await _context.ProductCatalogs
                .FirstOrDefaultAsync(c => c.CatalogId == catalogId);
        }

        public async Task<ProductCatalog> CreateAsync(CatalogVM catalog)
        {
            ProductCatalog newCatalog = new ProductCatalog
            {
                CatalogId = catalog.CatalogId,
                CatalogName = catalog.CatalogName
            };

            _context.ProductCatalogs.Add(newCatalog);
            await _context.SaveChangesAsync();
            return newCatalog;
        }

        public async Task<ProductCatalog?> UpdateAsync(CatalogVM catalog)
        {
            ProductCatalog? oldCatalog = await GetByIdAsync(catalog.CatalogId);
            if (oldCatalog == null)
            {
                return null;
            }

            oldCatalog.CatalogName = catalog.CatalogName;
            if(await _context.SaveChangesAsync() > 0)
            {
                return oldCatalog;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(string catalogId)
        {
            ProductCatalog? catalog = await GetByIdAsync(catalogId);
            if (catalog == null)
            {
                return false;
            }

            _context.ProductCatalogs.Remove(catalog);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
