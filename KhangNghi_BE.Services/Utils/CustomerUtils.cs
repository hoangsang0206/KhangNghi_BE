using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class CustomerUtils
    {
        public static IQueryable<Customer> SelectCustomer(this IQueryable<Customer> customers)
        {
            return customers
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    CompanyName = c.CompanyName,
                    CusTypeId = c.CusTypeId,
                    Email = c.Email,
                    FaxNumber = c.FaxNumber,
                    Gender = c.Gender,
                    PhoneNumber = c.PhoneNumber,
                    PositionInCompany = c.PositionInCompany,
                    MemberSince = c.MemberSince,
                    TaxCode = c.TaxCode,
                    Address = new Address
                    {
                        AddressId = c.Address.AddressId,
                        Street = c.Address.Street,
                        Ward = c.Address.Ward,
                        District = c.Address.District,
                        City = c.Address.City
                    }
                });
        }
    }
}
