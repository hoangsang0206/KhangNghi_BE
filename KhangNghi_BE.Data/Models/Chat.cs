using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Chat
{
    public string ChatId { get; set; } = null!;

    public string? ChatName { get; set; }

    public bool? IsGroupChat { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
