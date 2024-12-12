using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private KhangNghiContext _context;

        public DepartmentService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(DepartmentVM department)
        {
            Department newDepartment = new Department
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Location = department.Location
            };

            _context.Departments.Add(newDepartment);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Department? department = await  GetByIdAsync(id);
            if (department != null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Department>> GetAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(string id)
        {
            return await _context.Departments
                .Where(d => d.DepartmentId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(DepartmentVM department)
        {
            Department? departmentToUpdate = await GetByIdAsync(department.DepartmentId);
            if (departmentToUpdate == null)
            {
                return false;
            }

            departmentToUpdate.DepartmentName = department.DepartmentName;
            departmentToUpdate.Location = department.Location;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
