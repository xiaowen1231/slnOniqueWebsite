namespace prjOniqueWebsite.Models.DTOs
{
    public partial class EmployeeListDto
    {
        public int EmployeeId { get; set; }
        public string? PhotoPath { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? EmployeeLevelName { get; set; }


    }
}
