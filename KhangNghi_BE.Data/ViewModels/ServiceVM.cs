using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class ServiceVM
    {
        [Required(ErrorMessage = "Mã dịch vụ không để trống")]
        [MaxLength(50, ErrorMessage = "Mã danh mục không quá 50 kí tự")]
        [RegularExpression(@"^[a-zA-Z0-9-_]*$",
            ErrorMessage = "Mã danh mục không chứa kí tự đặc biệt (ngoại trừ -, _) và khoảng trống")]
        public string ServiceId { get; set; } = null!;

        [Required(ErrorMessage = "Tên dịch vụ không để trống")]
        public string ServiceName { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá dịch vụ không để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá dịch vụ phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string? CalculationUnit { get; set; }

        public string? CatalogId { get; set; }
    }
}
