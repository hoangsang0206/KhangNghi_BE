using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ContractDetail
{
    public string ContractId { get; set; } = null!;

    public string? ProductId { get; set; }

    public string? ServiceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual Service? Service { get; set; }
}
