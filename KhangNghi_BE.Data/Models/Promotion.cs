using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Promotion
{
    public string PromotionId { get; set; } = null!;

    public string PromotionName { get; set; } = null!;

    public string PromotionType { get; set; } = null!;

    public decimal? DiscountAmount { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
