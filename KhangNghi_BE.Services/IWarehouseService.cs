using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IWarehouseService
    {
        #region CRUD Warehouse
        Task<IEnumerable<Warehouse>> GetAsync(string? sortBy);
        Task<Warehouse?> GetByIdAsync(string id);
        Task<bool> CreateAsync(WarehouseVM warehouse);
        Task<bool> UpdateAsync(WarehouseVM warehouse);
        Task<bool> DeleteAsync(string id);
        #endregion

        #region Warehouse Stock
        Task<int> GetInStockQuantity(string productId);
        Task<int> GetInStockQuantity(string productId, string warehouseId);
        #endregion

        #region Warehouse Import
        Task<PagedList<StockEntry>> GetImportSlipsAsync(string? warehouseId, string? sortBy, int page, int pageSize);
        Task<IEnumerable<StockEntry>> GetStockEntriesBySupplierIdAsync(string supplierId);
        Task<StockEntry?> GetImportSlipByIdAsync(string id);
        Task<bool> CreateImportSlipAsync(StockEntry slip);
        #endregion
    }
}
