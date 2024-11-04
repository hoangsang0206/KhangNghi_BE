using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ChatMember
{
    public string ChatId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime? JoinedDate { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
