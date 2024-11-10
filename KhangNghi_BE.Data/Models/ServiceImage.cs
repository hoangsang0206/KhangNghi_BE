using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class ServiceImage
{
    public int Id { get; set; }

    public string ServiceId { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public virtual Service Service { get; set; } = null!;
}
