using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ProductSpecification
{
    public int Id { get; set; }

    public string SpecName { get; set; } = null!;

    public string SpecValue { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
