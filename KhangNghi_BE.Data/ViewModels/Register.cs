using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels;

public class Register
{
    [Required(ErrorMessage = "Tên đăng nhập không để trống")]
    [MaxLength(30, ErrorMessage = "Tên đăng nhập không quá 30 ký tự")] 
    public string Username { get; set; } = null!;
    
    [Required(ErrorMessage = "Email không để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Mật khẩu không để trống")]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
    [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
    public string ConfirmPassword { get; set; } = null!;
}