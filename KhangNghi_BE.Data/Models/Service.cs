using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Service
{
    public string ServiceId { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? CalculationUnit { get; set; }

    public string? CatalogId { get; set; }

    public virtual ServiceCatalog? Catalog { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual ICollection<ServiceImage> ServiceImages { get; set; } = new List<ServiceImage>();
}
