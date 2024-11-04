using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class EmployeePosition
{
    public string PositionId { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
