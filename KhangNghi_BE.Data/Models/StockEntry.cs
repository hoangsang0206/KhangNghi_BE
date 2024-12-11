using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class StockEntry
{
    public string EntryId { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? Note { get; set; }

    public string SupplierId { get; set; } = null!;

    public string WarehouseId { get; set; } = null!;

    public decimal TotalAmout { get; set; }

    public virtual ICollection<StockEntryDetail> StockEntryDetails { get; set; } = new List<StockEntryDetail>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
