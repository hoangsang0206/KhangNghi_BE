using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class SupplierVM
    {
        [Required(ErrorMessage = "Mã nhà cung cấp không để trống")]
        [RegularExpression(@"^[a-zA-Z0-9-_]*$",
            ErrorMessage = "Mã kho không chứa kí tự đặc biệt (ngoại trừ -, _) và khoảng trống")]
        public string SupplierId { get; set; } = null!;

        [Required(ErrorMessage = "Tên nhà cung cấp không để trống")]
        public string SupplierName { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không để trống")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Phường/xã không để trống")]
        public string Ward { get; set; } = null!;

        [Required(ErrorMessage = "Quận/huyện không để trống")]
        public string District { get; set; } = null!;

        [Required(ErrorMessage = "Tỉnh/thành phố không để trống")]
        public string City { get; set; } = null!;
    }
}
