using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class WorkSchedule
{
    public int SheduleId { get; set; }

    public int ShiftId { get; set; }

    public DateOnly WorkDate { get; set; }

    public string? Note { get; set; }

    public int? AssignmentId { get; set; }

    public virtual JobAssignment? Assignment { get; set; }

    public virtual Shift Shift { get; set; } = null!;
}
