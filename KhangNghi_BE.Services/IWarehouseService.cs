using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IWarehouseService
    {
        Task<IEnumerable<Warehouse>> GetAsync(string? sortBy);
        Task<Warehouse?> GetByIdAsync(string id);

        Task<bool> CreateAsync(WarehouseVM warehouse);
        Task<bool> UpdateAsync(WarehouseVM warehouse);
        Task<bool> DeleteAsync(string id);
    }
}
