using KhangNghi_BE.Data;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Contants;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly KhangNghiContext _context;

        public InvoiceService(KhangNghiContext context) => _context = context;

        public async Task<PagedList<Invoice>> GetAsync(string? filterBy, string? sortBy, int page, int pageSize)
        {
            return await _context.Invoices
                .ToPagedListAsync(page, pageSize);
        }

        public async Task<Invoice?> GetByIdAsync(string id)
        {
            return await _context.Invoices
                .Where(i => i.InvoiceId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> GetByContractId(string contractId)
        {
            return await _context.Invoices
                .Where(i => i.ContractId == contractId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PaidAsync(string id)
        {
            Invoice? invoice = await GetByIdAsync(id);
            if (invoice == null)
            {
                return false;
            }

            invoice.PaidDate = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateAsync(InvoiceVM invoice, string invoiceId)
        {
            Invoice _invoice = new Invoice
            {
                InvoiceId = invoiceId,
                ContractId = invoice.ContractId,
                EmployeeId = invoice.EmployeeId
            };

            IEnumerable<ContractDetail> cDetails = await _context.ContractDetails
                .Where(c => c.ContractId == invoice.ContractId)
                .ToListAsync();

            if(!cDetails.Any())
            {
                return false;
            }

            foreach(ContractDetail detail in cDetails)
            {
                _invoice.SubTotal += detail.Quantity * detail.UnitPrice;
            }

            _invoice.TotalAmout = _invoice.SubTotal;

            if(invoice.PromotionIds != null)
            {
                IEnumerable<Promotion> promotions = await _context.Promotions
                    .Where(p => invoice.PromotionIds.Contains(p.PromotionId))
                    .ToListAsync();

                foreach(Promotion promotion in promotions)
                {
                    if (promotion != null && promotion.IsActive
                        && promotion.StartDate >= DateTime.Now)
                    {
                        _invoice.PromotionUsages.Add(new PromotionUsage
                        {
                            PromotionId = promotion.PromotionId,
                            Promotion = promotion,
                            UsedAt = DateTime.Now
                        });

                        decimal discountAmount = 0;

                        if (promotion.PromotionType == PromotionContants.Percent)
                        {
                            discountAmount = _invoice.TotalAmout * (promotion.DiscountAmount ?? 0);
                            
                        }
                        else if (promotion.PromotionType == PromotionContants.Amount)
                        {
                            discountAmount = promotion.DiscountAmount ?? 0;
                        }

                        if (discountAmount > promotion.MaxDiscountAmount)
                        {
                            discountAmount = (decimal)promotion.MaxDiscountAmount;
                        }

                        _invoice.TotalAmout -= discountAmount;
                    }
                }
            }

            _invoice.TotalAmout -= _invoice.TotalAmout * (decimal)_invoice.TaxPercent;

            await _context.Invoices.AddAsync(_invoice);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> UpdateAsync(InvoiceVM invoice, string invoiceId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Invoice? invoice = await GetByIdAsync(id);
            if (invoice == null)
            {
                return false;
            }

            _context.PromotionUsages.RemoveRange(invoice.PromotionUsages);
            _context.Invoices.Remove(invoice);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
