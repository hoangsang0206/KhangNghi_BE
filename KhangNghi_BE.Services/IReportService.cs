using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IReportService
    {
        Task<AllSummaryVM> GetAllSummaryAsync();
        Task<ReportVM> GetReportAsync(DateTime fromDate, DateTime toDate);
    }
}
