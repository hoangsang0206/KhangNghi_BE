using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class CustomerType
{
    public string CusTypeId { get; set; } = null!;

    public string CusTypeName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
