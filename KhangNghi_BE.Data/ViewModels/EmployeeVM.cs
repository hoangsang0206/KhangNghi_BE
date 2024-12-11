using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class EmployeeVM
    {
        [Required(ErrorMessage = "Họ tên nhân viên không để trống")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        [Required(ErrorMessage = "Giới tính không để trống")]
        public string Gender { get; set; } = null!;

        public string? ProfileImageỦrl { get; set; }

        [Required(ErrorMessage = "Ngày vào làm không để trống")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Vị trí công việc")]
        public string PositionId { get; set; } = null!;

        [Required(ErrorMessage = "Phòng ban không để trống")]
        public string DepartmentId { get; set; } = null!;

        [Required(ErrorMessage = "Trình độ học vấn không để trống")]
        public string EducationalLevel { get; set; } = null!;

        [Required(ErrorMessage = "Chuyên ngành không để trống")]
        public string Major { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không để trống")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Xã/phường không để trống")]
        public string Ward { get; set; } = null!;

        [Required(ErrorMessage = "Quận/huyện không để trống")]
        public string District { get; set; } = null!;

        [Required(ErrorMessage = "Tỉnh/thành phố không để trống")]
        public string City { get; set; } = null!;
    }
}
