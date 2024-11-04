using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Warehouse
{
    public string WarehouseId { get; set; } = null!;

    public string WarehouseName { get; set; } = null!;

    public string AddressId { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<ProductsInWarehouse> ProductsInWarehouses { get; set; } = new List<ProductsInWarehouse>();

    public virtual ICollection<StockEntry> StockEntries { get; set; } = new List<StockEntry>();

    public virtual ICollection<StockExit> StockExits { get; set; } = new List<StockExit>();
}
