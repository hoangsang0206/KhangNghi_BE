using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class FunctionCategory
{
    public string CateId { get; set; } = null!;

    public string CateName { get; set; } = null!;

    public virtual ICollection<Function> Functions { get; set; } = new List<Function>();
}
