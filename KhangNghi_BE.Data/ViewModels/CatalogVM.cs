using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class CatalogVM
    {
        [Required(ErrorMessage = "Mã danh mục không để trống")]
        [RegularExpression(@"^[a-zA-Z0-9-_]*$", 
            ErrorMessage = "Mã danh mục không chứa kí tự đặc biệt (ngoại trừ -, _) và khoảng trống")]
        public string CatalogId { get; set; } = null!;

        [Required(ErrorMessage = "Tên danh mục không để trống")]
        public string CatalogName { get; set; } = null!;
    }
}
