using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class ProductVM
    {
        [Required(ErrorMessage = "Mã sản phẩm không để trống")]
        [MaxLength(50, ErrorMessage = "Mã sản phẩm không quá 50 kí tự")]
        [RegularExpression(@"^[a-zA-Z0-9-_]*$",
            ErrorMessage = "Mã sản phẩm không chứa kí tự đặc biệt (ngoại trừ -, _) và khoảng trống")]
        public string ProductId { get; set; } = null!;

        [Required(ErrorMessage = "Tên sản phẩm không để trống")]
        public string ProductName { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Giá so sánh phải lớn hơn 0")]
        public decimal? OriginalPrice { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        public int? Warranty { get; set; }

        public string? Origin { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? CalculationUnit { get; set; }

        public string[]? CatalogIds { get; set; }

        public string[]? Images { get; set; } 

        public List<Specification> Specs { get; set; } = new List<Specification>();

        public class Specification
        {
            [Required(ErrorMessage = "Tên thông số không để trống")]
            public string SpecName { get; set; } = null!;

            [Required(ErrorMessage = "Giá trị thông số không để trống")]
            public string SpecValue { get; set; } = null!;
        }
    }
}
