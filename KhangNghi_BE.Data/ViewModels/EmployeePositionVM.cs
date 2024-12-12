using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class EmployeePositionVM
    {
        [Required(ErrorMessage = "Mã chức vụ không được để trống")]
        public string PositionId { get; set; } = null!;

        [Required(ErrorMessage = "Tên chức vụ không được để trống")]
        public string PositionName { get; set; } = null!;
    }
}
