using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class ContractUtils
    {
        public static IQueryable<Contract> SelectContract(this IQueryable<Contract> contracts)
        {
            return contracts.Select(c => new Contract
            {
                ContractId = c.ContractId,
                CreateAt = c.CreateAt,
                CustomerId = c.CustomerId,
                SignedAt = c.SignedAt,
                FileUrl = c.FileUrl,
                CategoryId = c.CategoryId,
                CompletedAt = c.CompletedAt,
                InvoiceId = c.InvoiceId,
                ContractDetails = c.ContractDetails.Select(cd => new ContractDetail
                {
                    ProductId = cd.ProductId,
                    ServiceId = cd.ServiceId,
                    Quantity = cd.Quantity,
                    UnitPrice = cd.UnitPrice
                }).ToList(),
                Customer = new Customer
                {
                    CustomerId = c.Customer.CustomerId,
                    FullName = c.Customer.FullName,
                    PhoneNumber = c.Customer.PhoneNumber,
                    Email = c.Customer.Email,
                    AddressId = c.Customer.AddressId,
                    Address = new Address
                    {
                        AddressId = c.Customer.Address.AddressId,
                        Street = c.Customer.Address.Street,
                        Ward = c.Customer.Address.Ward,
                        District = c.Customer.Address.District,
                        City = c.Customer.Address.City
                    }
                },
            });
        }

        public static IQueryable<Contract> SortBy(this IQueryable<Contract> contracts, string? value)
        {
            switch (value)
            {
                case "newest":
                    return contracts.OrderBy(c => c.CreateAt);
                case "oldest":
                    return contracts.OrderByDescending(c => c.CreateAt);
                default:
                    return contracts;
            }
        }

        public static IQueryable<Contract> FilterBy(this IQueryable<Contract> contracts, string? value)
        {
            switch (value)
            {
                case "incomplete":
                    return contracts.Where(c => c.CompletedAt == null);
                case "completed":
                    return contracts.Where(c => c.CompletedAt != null);
                default:
                    return contracts;
            }
        }
    }
}
