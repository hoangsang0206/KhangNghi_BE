namespace KhangNghi_BE.Data.ViewModels
{
    public class ReportVM
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalContracts { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalSoldProductQuantity { get; set; }
        public int TotalProvidedServices { get; set; }
        public List<MonthReport> MonthReports { get; set; } = new List<MonthReport>();
    }

    public class MonthReport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int NewCustomers { get; set; }
        public int TotalContracts { get; set; }
        public int TotalSoldProductQuantity { get; set; }
        public int TotalProvidedServices { get; set; }
        public int TotalRevenue { get; set; }
    }
}
