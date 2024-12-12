using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly KhangNghiContext _context;

        public EmployeeService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(EmployeeVM employee, string id)
        {
            Employee newEmployee = new Employee
            {
                EmployeeId = id,
                FullName = employee.FullName,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Gender = employee.Gender,
                EducationalLevel = employee.EducationalLevel,
                HireDate = employee.HireDate,
                Major = employee.Major,
                ProfileImage = employee.ProfileImageỦrl,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId,
                Address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    Street = employee.Street,
                    Ward = employee.Ward,
                    District = employee.District,
                    City = employee.City
                }
            };

            _context.Employees.Add(newEmployee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var employee = await _context.Employees
                    .Where(e => e.EmployeeId == id)
                    .Include(e => e.Invoices)
                    .Include(e => e.Address)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    return false;
                }

                if (employee.Invoices.Any())
                {
                    return false;
                }

                _context.Addresses.Remove(employee.Address);
                _context.Employees.Remove(employee);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PagedList<Employee>> GetAsync(string? sortBy, int page, int pageSize)
        {
            return await _context.Employees
                .Select(employee => new Employee
                {
                    EmployeeId = employee.EmployeeId,
                    FullName = employee.FullName,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                    Gender = employee.Gender,
                    EducationalLevel = employee.EducationalLevel,
                    HireDate = employee.HireDate,
                    Major = employee.Major,
                    ProfileImage = employee.ProfileImage,
                    DepartmentId = employee.DepartmentId,
                    PositionId = employee.PositionId,
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid().ToString(),
                        Street = employee.Address.Street,
                        Ward = employee.Address.Ward,
                        District = employee.Address.District,
                        City = employee.Address.City
                    },
                    Position = new EmployeePosition
                    {
                        PositionId = employee.Position.PositionId,
                        PositionName = employee.Position.PositionName
                    },
                    Department = new Department
                    {
                        DepartmentId = employee.Department.DepartmentId,
                        DepartmentName = employee.Department.DepartmentName
                    }
                })
                .ToPagedListAsync(page, pageSize);
        }

        public Task<Employee?> GetByIdAsync(string id)
        {
            return _context.Employees
                .Where(e => e.EmployeeId == id)
                .SelectEmployee()
                .FirstOrDefaultAsync();
        }

        public Task<Employee?> GetByUserIdAsync(string userId)
        {
            return _context.Employees
                .Where(e => e.Users.Any(u => u.UserId == userId))
                .SelectEmployee()
                .FirstOrDefaultAsync();
        }


        public async Task<bool> UpdateAsync(EmployeeVM employee, string id)
        {
            Employee? existedEmployee = await _context.Employees
                .Where(e => e.EmployeeId == id)
                .Include(e => e.Address)
                .FirstOrDefaultAsync();

            if (existedEmployee == null)
            {
                return false;

            }

            existedEmployee.FullName = employee.FullName;
            existedEmployee.PhoneNumber = employee.PhoneNumber;
            existedEmployee.Email = employee.Email;
            existedEmployee.HireDate = employee.HireDate;
            existedEmployee.PositionId = employee.PositionId;
            existedEmployee.DepartmentId = employee.DepartmentId;
            existedEmployee.EducationalLevel = employee.EducationalLevel;
            existedEmployee.Major = employee.Major;
            existedEmployee.Address.Street = employee.Street;
            existedEmployee.Address.Ward = employee.Ward;
            existedEmployee.Address.District = employee.District;
            existedEmployee.Address.City = employee.City;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
