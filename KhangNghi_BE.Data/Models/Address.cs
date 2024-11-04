using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Address
{
    public string AddressId { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string District { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<JobAssignment> JobAssignments { get; set; } = new List<JobAssignment>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
