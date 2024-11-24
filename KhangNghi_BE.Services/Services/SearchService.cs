using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Services
{
    public class SearchService
    {
        private readonly KhangNghiContext _context;

        public SearchService(KhangNghiContext context) => _context = context;
    }
}
