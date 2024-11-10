using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal? OriginalPrice { get; set; }

    public decimal Price { get; set; }

    public int? Warranty { get; set; }

    public string? Origin { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? CalculationUnit { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; } = new List<ProductSpecification>();

    public virtual ICollection<ProductsInWarehouse> ProductsInWarehouses { get; set; } = new List<ProductsInWarehouse>();

    public virtual ICollection<ProductCatalog> Catalogs { get; set; } = new List<ProductCatalog>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
