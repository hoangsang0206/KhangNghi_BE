using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class FunctionAuthorization
{
    public int AuthId { get; set; }

    public string GroupId { get; set; } = null!;

    public string FunctionId { get; set; } = null!;

    public bool? IsAuthorized { get; set; }

    public virtual Function Function { get; set; } = null!;

    public virtual UserGroup Group { get; set; } = null!;
}
