using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ProductCatalog
{
    public string CatalogId { get; set; } = null!;

    public string CatalogName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
