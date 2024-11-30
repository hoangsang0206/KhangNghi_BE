using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace KhangNghi_BE.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly KhangNghiContext _context;

        public CustomerService(KhangNghiContext context) => _context = context;

        public async Task<PagedList<Customer>> GetAsync(string? sortBy, string? filterBy, int page, int pageSize)
        {
            return await _context.Customers
                .Include(c => c.Address)
                .Where(c => string.IsNullOrEmpty(filterBy) || c.FullName.Contains(filterBy))
                .OrderBy(c => sortBy)
                .ToPagedListAsync(page, pageSize);
        }

        public Task<Customer?> GetByIdAsync(string id)
        {
            return _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<IEnumerable<CustomerType>> GetCustomerTypesAsync()
        {
            return await _context.CustomerTypes.ToListAsync();
        }

        public async Task<CustomerType?> GetCustomerTypeAsync(string id)
        {
            return await _context.CustomerTypes
                .FirstOrDefaultAsync(c => c.CusTypeId == id);
        }

        public async Task<IEnumerable<Customer>> SearchByPhoneAsync(string phone)
        {
            return await _context.Customers
                .Include(c => c.Address)
                .Where(c => c.PhoneNumber.Contains(phone))
                .ToListAsync();
        }


        public async Task<bool> CreateAsync(CustomerVM customer, string customerId)
        {
            Customer newCustomer = new Customer
            {
                CustomerId = customerId,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                FaxNumber = customer.FaxNumber,
                Gender = customer.Gender,
                Email = customer.Email,
                CusTypeId = customer.CusTypeId,
                MemberSince = DateTime.Now,
                CompanyName = customer.CompanyName,
                TaxCode = customer.TaxCode,
                PositionInCompany = customer.PositionInCompany,
                Address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    Street = customer.Street,
                    Ward = customer.Ward,
                    District = customer.District,
                    City = customer.City,
                }
            };

            await _context.Customers.AddAsync(newCustomer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(string id, CustomerVM customer)
        {
            Customer? existingCustomer = await GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return false;
            }

            existingCustomer.FullName = customer.FullName;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.FaxNumber = customer.FaxNumber;
            existingCustomer.Gender = customer.Gender;
            existingCustomer.Email = customer.Email;
            existingCustomer.CusTypeId = customer.CusTypeId;
            existingCustomer.CompanyName = customer.CompanyName;
            existingCustomer.TaxCode = customer.TaxCode;
            existingCustomer.PositionInCompany = customer.PositionInCompany;
            existingCustomer.Address.Street = customer.Street;
            existingCustomer.Address.Ward = customer.Ward;
            existingCustomer.Address.District = customer.District;
            existingCustomer.Address.City = customer.City;
            
            _context.Customers.Update(existingCustomer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Customer? existingCustomer = await GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return false;
            }

            _context.Customers.Remove(existingCustomer);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
