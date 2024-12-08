﻿using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;

namespace KhangNghi_BE.Services
{
    public interface IContractService
    {
        Task<PagedList<Contract>> GetAsync(string? sortBy, string filterBy, int page, int pageSize);
        Task<Contract?> GetByIdAsync(string id);

        Task<bool> CreateAsync(ContractVM contract, string fileUrl);
        Task<bool> UpdateAsync(ContractVM contract);
        Task<bool> DeleteAsync(string id);

        Task<bool> SignContractAsync(string id);
    }
}
