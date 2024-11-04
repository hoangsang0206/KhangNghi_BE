using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class PromotionUsage
{
    public int UsageId { get; set; }

    public string PromotionId { get; set; } = null!;

    public string? InvoiceId { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime UsedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }

    public virtual Promotion Promotion { get; set; } = null!;
}
