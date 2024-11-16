using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class WarehouseVM
    {
        [Required(ErrorMessage = "Mã kho không được để trống")]
        [MaxLength(50, ErrorMessage = "Mã kho không quá 50 kí tự")]
        [RegularExpression(@"^[a-zA-Z0-9-_]*$",
            ErrorMessage = "Mã kho không chứa kí tự đặc biệt (ngoại trừ -, _) và khoảng trống")]
        public string WarehouseId { get; set; } = null!;

        [Required(ErrorMessage = "Tên kho không được để trống")]
        public string WarehouseName { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Phường/xã không được để trống")]
        public string Ward { get; set; } = null!;

        [Required(ErrorMessage = "Quận/huyện không được để trống")]
        public string District { get; set; } = null!;

        [Required(ErrorMessage = "Tỉnh/thành phố không được để trống")]
        public string City { get; set; } = null!;
    }
}
