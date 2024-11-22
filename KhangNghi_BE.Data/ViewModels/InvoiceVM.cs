using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class InvoiceVM
    {
        public string? Note { get; set; }

        [Required(ErrorMessage = "Mã thuế không để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "% thuế phải lớn hơn hoặc bằng 0" )]
        public double TaxPercent { get; set; }

        [Required(ErrorMessage = "Thông tin nhân viên tạo hóa đơn không để trống")]
        public string EmployeeId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn hợp đồng")]
        public string ContractId { get; set; } = null!;

        public string[]? PromotionIds { get; set; }
    }
}
