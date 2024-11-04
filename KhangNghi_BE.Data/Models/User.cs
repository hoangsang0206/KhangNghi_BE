using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public string? AvatarUrl { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsOnline { get; set; }

    public DateTime? LastLoggedIn { get; set; }

    public string RoleId { get; set; } = null!;

    public string? EmployeeId { get; set; }

    public string? CustomerId { get; set; }

    public string? GroupId { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual UserGroup? Group { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role Role { get; set; } = null!;
}
