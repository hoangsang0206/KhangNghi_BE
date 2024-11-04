using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string Gender { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public string AddressId { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public string PositionId { get; set; } = null!;

    public string DepartmentId { get; set; } = null!;

    public string EducationalLevel { get; set; } = null!;

    public string Major { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual EmployeePosition Position { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
