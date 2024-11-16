using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services.Services
{
    public class WarehouseService : IWarehouseService
    {
        public Task<bool> CreateAsync(WarehouseVM warehouse)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Warehouse>> GetAsync(string? sortBy)
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(WarehouseVM warehouse)
        {
            throw new NotImplementedException();
        }
    }
}
