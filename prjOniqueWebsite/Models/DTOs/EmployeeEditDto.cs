namespace prjOniqueWebsite.Models.DTOs
{
    public class EmployeeEditDto
    {
        public int EmployeeId { get; set; }
        public string? PhotoPath { get; set; }
        public string EmployeeName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string? Address { get; set; }
        public string EmployeeLevel { get; set; }
        public string? RegisterDate { get; set; }
        public string DateOfBirth { get; set; }
    }
}
