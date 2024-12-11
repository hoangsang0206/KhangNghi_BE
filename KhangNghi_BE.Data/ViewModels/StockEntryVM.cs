using KhangNghi_BE.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class StockEntryVM
    {
        public string? Note { get; set; }

        [Required(ErrorMessage = "Mã nhà cung cấp không để trống")]
        public string SupplierId { get; set; } = null!;

        [Required(ErrorMessage = "Mã kho hàng không để trống")]
        public string WarehouseId { get; set; } = null!;

        public virtual List<StockEntryDetailVM> StockEntryDetails { get; set; } = new List<StockEntryDetailVM>();
    }

    public class StockEntryDetailVM
    {
        public string ProductId { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

    }
}
