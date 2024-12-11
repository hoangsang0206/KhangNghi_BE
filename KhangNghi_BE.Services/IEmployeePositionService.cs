using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services
{
    public interface IEmployeePositionService
    {
        Task<IEnumerable<EmployeePosition>> GetAsync();
        Task<EmployeePosition> GetByIdAsync(string id);

        Task<bool> CreateAsync(EmployeePosition employeePosition);
        Task<bool> UpdateAsync(string id, EmployeePosition employeePosition);
        Task<bool> DeleteAsync(string id);
    }
}
