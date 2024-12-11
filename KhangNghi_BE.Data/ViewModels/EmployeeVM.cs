namespace KhangNghi_BE.Data.ViewModels
{
    public class EmployeeVM
    {
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        public string Gender { get; set; } = null!;

        public string? ProfileImageỦrl { get; set; }

        public DateTime HireDate { get; set; }

        public string PositionId { get; set; } = null!;

        public string DepartmentId { get; set; } = null!;

        public string EducationalLevel { get; set; } = null!;

        public string Major { get; set; } = null!;

        public string Street { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string District { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
