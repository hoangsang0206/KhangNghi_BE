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

        public ReportVM GetReport(DateTime fromDate, DateTime toDate)
        {
            var months = Enumerable.Range(0, (toDate.Year - fromDate.Year) * 12 + toDate.Month - fromDate.Month + 1)
                .Select(i => fromDate.AddMonths(i))
                .Select(i => new { i.Year, i.Month })
                .ToList();

            var monthReports = months.Select(m =>
            {
                var newCustomers = _context.Customers
                    .Count(c => c.MemberSince.HasValue
                        && c.MemberSince.Value.Date >= fromDate.Date
                        && c.MemberSince.Value.Date <= toDate.Date
                        && c.MemberSince.Value.Year == m.Year && c.MemberSince.Value.Month == m.Month);

                var contracts = _context.Contracts
                    .Where(c => c.CreateAt.HasValue
                        && c.CreateAt.Value.Date >= fromDate.Date
                        && c.CreateAt.Value.Date <= toDate.Date
                        && c.CreateAt.Value.Year == m.Year && c.CreateAt.Value.Month == m.Month)
                    .Include(c => c.ContractDetails)
                    .Include(c => c.Invoice)
                    .ToList();

                int totalContracts = contracts.Count();
                int totalSoldProductQuantity = contracts.Sum(c => c.ContractDetails
                        .Where(cd => cd.ProductId != null).Sum(cd => cd.Quantity));
                int totalProvidedServices = contracts.Sum(c => c.ContractDetails
                        .Count(cd => cd.ServiceId != null));

                return new MonthReport
                {
                    Month = m.Month,
                    Year = m.Year,
                    NewCustomers = newCustomers,
                    TotalContracts = totalContracts,
                    TotalSoldProductQuantity = totalSoldProductQuantity,
                    TotalProvidedServices = totalProvidedServices,
                    TotalRevenue = contracts.Sum(c => c.Invoice?.TotalAmout ?? 0)
                };
            }).ToList();

            return new ReportVM
            {
                FromDate = fromDate,
                ToDate = toDate,
                TotalContracts = monthReports.Sum(m => m.TotalContracts),
                TotalRevenue = monthReports.Sum(m => m.TotalRevenue),
                TotalSoldProductQuantity = monthReports.Sum(m => m.TotalSoldProductQuantity),
                TotalProvidedServices = monthReports.Sum(m => m.TotalProvidedServices),
                MonthReports = monthReports
            };
        }
    }
}
