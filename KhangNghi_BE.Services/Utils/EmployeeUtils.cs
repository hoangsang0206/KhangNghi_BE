using KhangNghi_BE.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhangNghi_BE.Services.Utils
{
    public static class EmployeeUtils
    {
        public static IQueryable<Employee> SelectEmployee(this IQueryable<Employee> employees)
        {
            return employees.Select(employee => new Employee
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
            });
        }
    }
}
