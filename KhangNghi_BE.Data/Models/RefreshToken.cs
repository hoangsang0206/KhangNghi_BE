using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class RefreshToken
{
    public string UserId { get; set; } = null!;

    public string JwtId { get; set; } = null!;

    public string? Token { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime ExpireAt { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
