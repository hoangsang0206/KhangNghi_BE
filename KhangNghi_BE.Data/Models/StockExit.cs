using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class StockExit
{
    public string ExitId { get; set; } = null!;

    public DateTime ExitDate { get; set; }

    public string? Note { get; set; }

    public decimal TotalAmout { get; set; }

    public string WarehouseId { get; set; } = null!;

    public string? ContractId { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<StockExitDetail> StockExitDetails { get; set; } = new List<StockExitDetail>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
