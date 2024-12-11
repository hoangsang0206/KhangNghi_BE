using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private KhangNghiContext _context;

        public DepartmentService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Department department)
        {

            return false;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Department>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Department> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, Department department)
        {
            throw new NotImplementedException();
        }
    }
}
