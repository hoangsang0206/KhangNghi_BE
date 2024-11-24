using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Data.ViewModels
{
    public class SearchResultVM
    {
        IEnumerable<Product> Products { get; set; } = new List<Product>();

        IEnumerable<Service> Services { get; set; } = new List<Service>();
    }
}
