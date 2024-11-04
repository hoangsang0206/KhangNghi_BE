using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class PromotionProduct
{
    public string PromotionId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
