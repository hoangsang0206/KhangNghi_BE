using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Department
{
    public string DepartmentId { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public string? Location { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
