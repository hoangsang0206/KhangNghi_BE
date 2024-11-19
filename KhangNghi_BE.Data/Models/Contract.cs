using System;
using System.Collections.Generic;

namespace KhangNghi_BE.Data.Models;

public partial class Contract
{
    public string ContractId { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public DateTime? SignedAt { get; set; }

    public string FileUrl { get; set; } = null!;

    public string? CategoryId { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime? CompletedAt { get; set; }

    public virtual ContractCategory? Category { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<JobAssignment> JobAssignments { get; set; } = new List<JobAssignment>();

    public virtual ICollection<StockExit> StockExits { get; set; } = new List<StockExit>();
}
