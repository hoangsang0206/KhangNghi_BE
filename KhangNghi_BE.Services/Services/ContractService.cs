using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class ContractService : IContractService
    {
        private readonly KhangNghiContext _context;
        
        public ContractService(KhangNghiContext context) => _context = context;

        public async Task<PagedList<Contract>> GetAsync(string? sortBy, string filterBy, int page, int pageSize)
        {
            return await _context.Contracts
                .FilterBy(filterBy)
                .SortBy(sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<Contract?> GetByIdAsync(string id)
        {
            return await _context.Contracts
                .Where(c => c.ContractId == id)
                .Include(c => c.ContractDetails)
                .Include(c => c.Customer)
                .Include(c => c.Invoice)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(ContractVM contract)
        {
            Contract _contract = new Contract
            {
                ContractId = contract.ContractId,
                CreateAt = DateTime.Now,
                CustomerId = contract.CustomerId,
                SignedAt = contract.SignedAt,
                FileUrl = contract.FileUrl,

                ContractDetails = contract.ContractDetails.Select(cd =>
                {
                    ContractDetail detail = new ContractDetail
                    {
                        UnitPrice = cd.UnitPrice,
                        Quantity = cd.Quantity
                    };

                    if (cd.ServiceId != null)
                    {
                        detail.ServiceId = cd.ServiceId;
                    } 
                    else
                    {
                        detail.ProductId = cd.ProductId;
                    }  
                    
                    return detail;
                }).ToList()
            };

            await _context.Contracts.AddAsync(_contract);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ContractVM contract)
        {
            Contract? _contract = await GetByIdAsync(contract.ContractId);
            if (_contract == null)
            {
                return false;
            }

            // Update necessary fields here

            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Contract? contract = await GetByIdAsync(id);
            if (contract == null)
            {
                return false;
            }

            if(contract.Invoice != null)
            {
                PromotionUsage? promotionUsage = await _context.PromotionUsages
                    .Where(pu => pu.InvoiceId == contract.Invoice.InvoiceId)
                    .FirstOrDefaultAsync();

                if (promotionUsage != null)
                {
                    _context.PromotionUsages.Remove(promotionUsage);
                }

                _context.Invoices.Remove(contract.Invoice);
            }

            _context.ContractDetails.RemoveRange(contract.ContractDetails);
            _context.Contracts.Remove(contract);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SignContractAsync(string id)
        {
            Contract? contract = await GetByIdAsync(id);
            if (contract == null)
            {
                return false;
            }

            contract.SignedAt = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
