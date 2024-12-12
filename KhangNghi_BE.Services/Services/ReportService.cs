using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly KhangNghiContext _context;

        public ReportService(KhangNghiContext context)
        {
            _context = context;
        }

        public async Task<AllSummaryVM> GetAllSummaryAsync()
        {
            return new AllSummaryVM
            {
                TotalContracts = await _context.Contracts.CountAsync(),
                TotalRevenue = await _context.Invoices.SumAsync(i => i.TotalAmout),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalServices = await _context.Services.CountAsync(),
                TotalWarehouses = await _context.Warehouses.CountAsync()
            };
        }
    }
}
