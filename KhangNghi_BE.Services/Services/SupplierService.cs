using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly KhangNghiContext _context;

        public SupplierService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(SupplierVM supplier)
        {
            Supplier newSupplier = new Supplier
            {
                SupplierId = supplier.SupplierId,
                SupplierName = supplier.SupplierName,
                PhoneNumber = supplier.PhoneNumber,
                Address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    Street = supplier.Street,
                    Ward = supplier.Ward,
                    District = supplier.District,
                    City = supplier.City
                }
            };

            await _context.Suppliers.AddAsync(newSupplier);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Supplier? supplier = await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Include(s => s.Address)
                .Include(s => s.StockEntries)
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                return false;
            }

            if (supplier.StockEntries.Any())
            {
                return false;
            }

            _context.Addresses.Remove(supplier.Address);
            _context.Suppliers.Remove(supplier);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .Select(s => new Supplier
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    PhoneNumber = s.PhoneNumber,
                    AddressId = s.AddressId,
                    Address = new Address
                    {
                        Street = s.Address.Street,
                        Ward = s.Address.Ward,
                        District = s.Address.District,
                        City = s.Address.City
                    }
                })
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(string id)
        {
            return await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Select(s => new Supplier
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    PhoneNumber = s.PhoneNumber,
                    AddressId = s.AddressId,
                    Address = new Address
                    {
                        Street = s.Address.Street,
                        Ward = s.Address.Ward,
                        District = s.Address.District,
                        City = s.Address.City
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(SupplierVM supplier)
        {
            Supplier? existedSupplier = await _context.Suppliers
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.SupplierId == supplier.SupplierId);

            if (existedSupplier == null)
            {
                return false;
            }

            existedSupplier.SupplierName = supplier.SupplierName;
            existedSupplier.PhoneNumber = supplier.PhoneNumber;
            existedSupplier.Address.Street = supplier.Street;
            existedSupplier.Address.Ward = supplier.Ward;
            existedSupplier.Address.District = supplier.District;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
