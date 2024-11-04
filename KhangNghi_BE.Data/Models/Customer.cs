using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? FaxNumber { get; set; }

    public string? Email { get; set; }

    public string Gender { get; set; } = null!;

    public DateTime? MemberSince { get; set; }

    public string CusTypeId { get; set; } = null!;

    public string AddressId { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string? PositionInCompany { get; set; }

    public string? TaxCode { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual CustomerType CusType { get; set; } = null!;

    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
