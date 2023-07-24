using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.DTOs
{
    public class EmployeeDto
    {
        public string EmployeeName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmployeeLevelName { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual Areas AreasNavigation { get; set; }
        public virtual Citys CitysNavigation { get; set; }

        public string Address { get; set; }
    }
}
