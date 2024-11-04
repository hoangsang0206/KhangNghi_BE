using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ServiceCatalog
{
    public string CatalogId { get; set; } = null!;

    public string CatalogName { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
