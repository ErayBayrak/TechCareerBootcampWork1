namespace EntityFramework.Models
{
    public class EmployeeListModel
    {
        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public DateTime? HireDate { get; set; }
        public string? Title { get; set; }
    }
}
