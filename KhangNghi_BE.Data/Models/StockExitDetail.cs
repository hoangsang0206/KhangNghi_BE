using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class StockExitDetail
{
    public string ProductId { get; set; } = null!;

    public string ExitId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int Id { get; set; }

    public virtual StockExit Exit { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
