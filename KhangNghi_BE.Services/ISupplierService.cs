using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhangNghi_BE.Services
{
    public interface ISupplierService
    {
        Task<Supplier?> GetByIdAsync(string id);
        Task<IEnumerable<Supplier>> GetAllAsync();

        Task<bool> CreateAsync(SupplierVM supplier);
        Task<bool> UpdateAsync(SupplierVM supplier);
        Task<bool> DeleteAsync(string id);
    }
}
