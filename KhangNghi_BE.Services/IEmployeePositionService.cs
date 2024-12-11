using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IEmployeePositionService
    {
        Task<IEnumerable<EmployeePosition>> GetAsync();
        Task<EmployeePosition?> GetByIdAsync(string id);

        Task<bool> CreateAsync(EmployeePositionVM employeePosition);
        Task<bool> UpdateAsync(EmployeePositionVM employeePosition);
        Task<bool> DeleteAsync(string id);
    }
}
