using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAsync();
        Task<Department> GetByIdAsync(string id);

        Task<bool> CreateAsync(Department department);
        Task<bool> UpdateAsync(string id, Department department);
        Task<bool> DeleteAsync(string id);
    }
}
