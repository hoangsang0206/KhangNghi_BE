using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public string? Note { get; set; }

    public double TaxPercent { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TotalAmout { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? EmployeeId { get; set; }

    public string ContractId { get; set; } = null!;

    public virtual Contract Contract { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();
}
