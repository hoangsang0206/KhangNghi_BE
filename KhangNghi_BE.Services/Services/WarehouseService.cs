using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly KhangNghiContext _context;

        public WarehouseService(KhangNghiContext context) => _context = context;

        #region CRUD Warehouse

        public async Task<IEnumerable<Warehouse>> GetAsync(string? sortBy)
        {
            return await _context.Warehouses
                .SelectWarehouse()
                .ToListAsync();
        }

        public Task<Warehouse?> GetByIdAsync(string id)
        {
            return _context.Warehouses
                .Where(w => w.WarehouseId == id)
                .SelectWarehouse()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(WarehouseVM warehouse)
        {
            await _context.Warehouses.AddAsync(new Warehouse
            {
                WarehouseId = warehouse.WarehouseId,
                WarehouseName = warehouse.WarehouseName,
                Address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    Street = warehouse.Street,
                    Ward = warehouse.Ward,
                    District = warehouse.District,
                    City = warehouse.City
                }
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(WarehouseVM warehouse)
        {
            Warehouse? _warehouse = await GetByIdAsync(warehouse.WarehouseId);
            if (_warehouse == null)
            {
                return false;
            }

            _warehouse.WarehouseName = warehouse.WarehouseName;
            _warehouse.Address.Street = warehouse.Street;
            _warehouse.Address.Ward = warehouse.Ward;
            _warehouse.Address.District = warehouse.District;
            _warehouse.Address.City = warehouse.City;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Warehouse? warehouse = await GetByIdAsync(id);
            if (warehouse == null)
            {
                return false;
            }

            _context.Warehouses.Remove(warehouse);
            return await _context.SaveChangesAsync() > 0;
        }

        #endregion

        #region Warehouse Stock
        
        public async Task<int> GetInStockQuantity(string productId)
        {
            return await _context.ProductsInWarehouses
                .Where(p => p.ProductId == productId)
                .SumAsync(p => p.Quantity);
        }

        public async Task<int> GetInStockQuantity(string productId, string warehouseId)
        {
            return await _context.ProductsInWarehouses
                .Where(p => p.ProductId == productId && p.WarehouseId == warehouseId)
                .Select(p => p.Quantity)
                .FirstOrDefaultAsync();
        }

        #endregion


        #region Warehouse Import

        public async Task<PagedList<StockEntry>> GetImportSlipsAsync(string? warehouseId, string? sortBy, int page, int pageSize)
        {
            IQueryable<StockEntry> query = warehouseId != null
                ? _context.StockEntries
                : _context.StockEntries.Where(s => s.WarehouseId == warehouseId);

            return await query.Select(s => new StockEntry
            {
                EntryId = s.EntryId,
                EntryDate = s.EntryDate,
                Note = s.Note,
                TotalAmout = s.TotalAmout,
                Supplier = new Supplier
                {
                    SupplierId = s.Supplier.SupplierId,
                    SupplierName = s.Supplier.SupplierName,
                },
                Warehouse = new Warehouse
                {
                    WarehouseId = s.Warehouse.WarehouseId,
                    WarehouseName = s.Warehouse.WarehouseName,
                },
                StockEntryDetails = s.StockEntryDetails.Select(d => new StockEntryDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList()
            })
            .OrderByDescending(s => s.EntryDate)
            .ToPagedListAsync(page, pageSize);
        }

        public async Task<IEnumerable<StockEntry>> GetStockEntriesBySupplierIdAsync(string supplierId)
        {
            return await _context.StockEntries
                .Where(s => s.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<StockEntry?> GetImportSlipByIdAsync(string id)
        {
            return await _context.StockEntries
                .Where(s => s.EntryId == id)
                .Select(s => new StockEntry
                {
                    EntryId = s.EntryId,
                    EntryDate = s.EntryDate,
                    Note = s.Note,
                    TotalAmout = s.TotalAmout,
                    Supplier = new Supplier
                    {
                        SupplierId = s.Supplier.SupplierId,
                        SupplierName = s.Supplier.SupplierName,
                    },
                    Warehouse = new Warehouse
                    {
                        WarehouseId = s.Warehouse.WarehouseId,
                        WarehouseName = s.Warehouse.WarehouseName,
                    },
                    StockEntryDetails = s.StockEntryDetails.Select(d => new StockEntryDetail
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateImportSlipAsync(StockEntry slip)
        {
            await _context.StockEntries.AddAsync(slip);
            bool result =  await _context.SaveChangesAsync() > 0;

            if (result)
            {
                foreach (var detail in slip.StockEntryDetails)
                {
                    var wProduct = await _context.ProductsInWarehouses
                        .Where(p => p.ProductId == detail.ProductId && p.WarehouseId == slip.WarehouseId)
                        .FirstOrDefaultAsync();

                    if (wProduct != null)
                    {
                        wProduct.Quantity += detail.Quantity;
                    }
                    else
                    {
                        await _context.ProductsInWarehouses.AddAsync(new ProductsInWarehouse
                        {
                            ProductId = detail.ProductId,
                            WarehouseId = slip.WarehouseId,
                            Quantity = detail.Quantity,
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            return result;
        }

        #endregion

        #region Warehouse Export

        public async Task<PagedList<StockExit>> GetExportSlipsAsync(string? warehouseId, string? sortBy, int page, int pageSize)
        {
            IQueryable<StockExit> query = warehouseId != null
                ? _context.StockExits
                : _context.StockExits.Where(s => s.WarehouseId == warehouseId);

            return await query.Select(s => new StockExit
            {
                ExitId = s.ExitId,
                ExitDate = s.ExitDate,
                Note = s.Note,
                TotalAmout = s.TotalAmout,
                ContractId = s.ContractId,
                Warehouse = new Warehouse
                {
                    WarehouseId = s.Warehouse.WarehouseId,
                    WarehouseName = s.Warehouse.WarehouseName,
                },
                StockExitDetails = s.StockExitDetails.Select(d => new StockExitDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                }).ToList()
            })
            .OrderByDescending(s => s.ExitDate)
            .ToPagedListAsync(page, pageSize);
        }

        public async Task<StockExit?> GetExportSlipByIdAsync(string id)
        {
            return await _context.StockExits
                .Where(s => s.ExitId == id)
                .Select(s => new StockExit
                {
                    ExitId = s.ExitId,
                    ExitDate = s.ExitDate,
                    Note = s.Note,
                    TotalAmout = s.TotalAmout,
                    ContractId = s.ContractId,
                    Warehouse = new Warehouse
                    {
                        WarehouseId = s.Warehouse.WarehouseId,
                        WarehouseName = s.Warehouse.WarehouseName,
                    },
                    StockExitDetails = s.StockExitDetails.Select(d => new StockExitDetail
                    {
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateExportSlipAsync(StockExit slip)
        {
            await _context.StockExits.AddAsync(slip);
            bool result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                foreach (var detail in slip.StockExitDetails)
                {
                    var wProduct = await _context.ProductsInWarehouses
                        .Where(p => p.ProductId == detail.ProductId && p.WarehouseId == slip.WarehouseId)
                        .FirstOrDefaultAsync();

                    if (wProduct != null)
                    {
                        wProduct.Quantity -= detail.Quantity;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return result;
        }

        #endregion
    }
}
