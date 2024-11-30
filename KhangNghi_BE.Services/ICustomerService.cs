using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface ICustomerService
    {
        Task<PagedList<Customer>> GetAsync(string? sortBy, string? filterBy, int page, int pageSize);
        Task<IEnumerable<Customer>> SearchByPhoneAsync(string phone);
        Task<Customer?> GetByIdAsync(string id);
        Task<bool> CreateAsync(CustomerVM customer, string customerId);
        Task<bool> UpdateAsync(string id, CustomerVM customer);
        Task<bool> DeleteAsync(string id);

        Task<IEnumerable<CustomerType>> GetCustomerTypesAsync();
        Task<CustomerType?> GetCustomerTypeAsync(string id);
    }
}
