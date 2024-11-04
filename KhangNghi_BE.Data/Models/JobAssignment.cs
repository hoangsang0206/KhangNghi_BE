using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class JobAssignment
{
    public int AssignmentId { get; set; }

    public string? JobName { get; set; }

    public string EmployeeId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? WorkAddressId { get; set; }

    public string? Note { get; set; }

    public string? ContractId { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Address? WorkAddress { get; set; }

    public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();
}
