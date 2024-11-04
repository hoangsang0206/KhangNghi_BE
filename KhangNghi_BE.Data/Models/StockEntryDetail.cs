using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class StockEntryDetail
{
    public string ProductId { get; set; } = null!;

    public string EntryId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual StockEntry Entry { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
