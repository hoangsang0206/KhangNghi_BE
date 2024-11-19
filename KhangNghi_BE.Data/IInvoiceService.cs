using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Data
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetByIdAsync(int id);
        Task<PagedList<Invoice>> GetAsync();
        Task<bool> CreateAsync(InvoiceVM invoice);
        Task<bool> UpdateAsync(InvoiceVM invoice);
        Task<bool> PaidAsync(string id);
        Task<bool> DeleteAsync(int id);
    }
}
