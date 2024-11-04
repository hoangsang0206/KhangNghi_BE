using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
