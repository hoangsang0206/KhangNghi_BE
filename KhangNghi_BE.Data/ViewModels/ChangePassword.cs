using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Mật khẩu cũ không để trống")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu mới không để trống")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Xác nhận mật khẩu không để trống")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
