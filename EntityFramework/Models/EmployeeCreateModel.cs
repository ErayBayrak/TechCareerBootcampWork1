namespace EntityFramework.Models
{
    public class EmployeeCreateModel
    {
        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? Title { get; set; }
        public DateTime? HireDate { get; set; }
        public int? ReportsTo { get; set; }
    }
}
