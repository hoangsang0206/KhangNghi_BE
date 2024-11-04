using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Function
{
    public string FuncId { get; set; } = null!;

    public string FuncName { get; set; } = null!;

    public virtual ICollection<FunctionAuthorization> FunctionAuthorizations { get; set; } = new List<FunctionAuthorization>();
}
