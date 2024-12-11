using KhangNghi_BE.Data.Models;
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
    }
}
