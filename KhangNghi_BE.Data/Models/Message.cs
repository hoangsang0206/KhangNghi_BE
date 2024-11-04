using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public string ChatId { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public bool? IsReceiverRead { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
