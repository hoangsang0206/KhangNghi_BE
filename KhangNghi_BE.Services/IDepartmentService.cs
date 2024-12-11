using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAsync();
        Task<Department?> GetByIdAsync(string id);

        Task<bool> CreateAsync(DepartmentVM department);
        Task<bool> UpdateAsync(DepartmentVM department);
        Task<bool> DeleteAsync(string id);
    }
}
