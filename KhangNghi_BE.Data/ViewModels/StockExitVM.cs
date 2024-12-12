using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Data.ViewModels
{
    public class StockExitVM
    {
        public string? Note { get; set; }

        public string WarehouseId { get; set; } = null!;

        public string? ContractId { get; set; }

        public List<StockExitDetailVM> StockExitDetails { get; set; } = new List<StockExitDetailVM>();
    }

    public class StockExitDetailVM
    {
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
