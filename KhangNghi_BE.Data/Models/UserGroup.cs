using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class UserGroup
{
    public string GroupId { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public bool? HasAllPermission { get; set; }

    public virtual ICollection<FunctionAuthorization> FunctionAuthorizations { get; set; } = new List<FunctionAuthorization>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
