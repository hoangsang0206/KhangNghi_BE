using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Data
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetByIdAsync(string id);
        Task<Invoice?> GetByContractId(string contractId);
        Task<PagedList<Invoice>> GetAsync(string? filterBy, string? sortBy, int page, int pageSize);

        Task<bool> CreateAsync(InvoiceVM invoice, string invoiceId);
        Task<bool> UpdateAsync(InvoiceVM invoice, string invoiceId);
        Task<bool> PaidAsync(string id);
        Task<bool> DeleteAsync(string id);
    }
}
