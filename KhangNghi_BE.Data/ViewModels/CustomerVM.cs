using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class CustomerVM
    {
        [Required(ErrorMessage = "Họ tên khách hàng không để trống")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string PhoneNumber { get; set; } = null!;

        public string? FaxNumber { get; set; }

        public string? Email { get; set; }

        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Loại khách hàng không để trống")]
        public string CusTypeId { get; set; } = null!;

        public string? CompanyName { get; set; }

        public string? PositionInCompany { get; set; }

        public string? TaxCode { get; set; }

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
