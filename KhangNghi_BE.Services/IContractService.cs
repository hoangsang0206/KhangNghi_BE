using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IContractService
    {
        Task<PagedList<Contract>> GetAsync(string? sortBy, string filterBy, int page, int pageSize);
        Task<IEnumerable<Contract>> GetByCustomerAsync(string customerId);
        Task<Contract?> GetByIdAsync(string id);

        Task<bool> CreateAsync(ContractVM contract);
        Task<bool> UpdateAsync(ContractVM contract);
        Task<bool> DeleteAsync(string id);

        Task<bool> SignContractAsync(string id);
    }
}
