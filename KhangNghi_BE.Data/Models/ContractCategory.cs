using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ContractCategory
{
    public string CategoryId { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
