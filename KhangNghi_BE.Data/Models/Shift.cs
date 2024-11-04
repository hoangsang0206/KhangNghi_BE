using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Shift
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();
}
