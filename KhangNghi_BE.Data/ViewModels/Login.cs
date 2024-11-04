using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhangNghi_BE.Data.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}
