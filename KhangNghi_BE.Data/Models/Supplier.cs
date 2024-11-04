using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Supplier
{
    public string SupplierId { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string AddressId { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<StockEntry> StockEntries { get; set; } = new List<StockEntry>();
}
