using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class DepartmentVM
    {
        [Required(ErrorMessage = "Mã phòng ban không để trống")]
        public string DepartmentId { get; set; } = null!;

        [Required(ErrorMessage = "Tên phòng ban không để trống")]
        public string DepartmentName { get; set; } = null!;

        public string? Location { get; set; }

    }
}
