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

        public async Task<ReportVM> GetReportAsync(DateTime fromDate, DateTime toDate)
        {
            var months = Enumerable.Range(0, (toDate.Year - fromDate.Year) * 12 + toDate.Month - fromDate.Month + 1)
                .Select(i => fromDate.AddMonths(i))
                .Select(i => new { i.Year, i.Month })
                .ToList();

            var monthReports = months.Select(m => new MonthReport
            {
                //Year = m.Year,
                //Month = m.Month,
                //NewCustomers = _context.Customers
                //    .Count(c => c.MemberSince != null && c.MemberSince.Year == m.Year && c.MemberSince.Month == m.Month),
                //TotalContracts = _context.Contracts.Count(c => c.CreatedAt.Year == m.Year && c.CreatedAt.Month == m.Month),
                //TotalSoldProductQuantity = _context.ContractDetails
                //    .Where(cd => cd.Contract.CreatedAt.Year == m.Year && cd.Contract.CreatedAt.Month == m.Month)
                //    .Sum(cd => cd.Quantity),
                //TotalProvidedServices = _context.ContractServices
                //    .Where(cs => cs.Contract.CreatedAt.Year == m.Year && cs.Contract.CreatedAt.Month == m.Month)
                //    .Count(),
                //TotalRevenue = _context.Invoices
                //    .Where(i => i.CreatedAt.Year == m.Year && i.CreatedAt.Month == m.Month)
                //    .Sum(i => i.TotalAmout)
            }).ToList();

            return new ReportVM
            {
                FromDate = fromDate,
                ToDate = toDate,
                
            };
        }
    }
}
