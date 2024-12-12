using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class EmployeePositionService : IEmployeePositionService
    {
        private readonly KhangNghiContext _context;

        public EmployeePositionService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(EmployeePositionVM employeePosition)
        {
            EmployeePosition position = new EmployeePosition
            {
                PositionId = employeePosition.PositionId,
                PositionName = employeePosition.PositionName
            };

            await _context.EmployeePositions.AddAsync(position);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var position = await GetByIdAsync(id);

            if (position == null)
            {
                return false;
            }

            _context.EmployeePositions.Remove(position);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<EmployeePosition>> GetAsync()
        {
            
            return await _context.EmployeePositions.ToListAsync();
        }

        public async Task<EmployeePosition?> GetByIdAsync(string id)
        {
            return await _context.EmployeePositions
                .FirstOrDefaultAsync(x => x.PositionId == id);
        }

        public async Task<bool> UpdateAsync(EmployeePositionVM employeePosition)
        {
            var position = await GetByIdAsync(employeePosition.PositionId);

            if (position == null)
            {
                return false;
            }

            position.PositionName = employeePosition.PositionName;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
