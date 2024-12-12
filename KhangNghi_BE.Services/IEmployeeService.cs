using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IEmployeeService
    {
        Task<PagedList<Employee>> GetAsync(string? sortBy, int page, int pageSize);
        Task<Employee?> GetByIdAsync(string id);

        Task<Employee?> GetByUserIdAsync(string userId);

        Task<bool> CreateAsync(EmployeeVM employee, string id);
        Task<bool> UpdateAsync(EmployeeVM employee, string id);
        Task<bool> DeleteAsync(string id);
    }
}
