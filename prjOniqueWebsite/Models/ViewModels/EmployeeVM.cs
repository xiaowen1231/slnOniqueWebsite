using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class EmployeeVM
    {
        [Required]
        public string EmployeeName { get; set; }
        public string PhotoPath { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string EmployeeLevel { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Citys { get; set; }
        public string Areas { get; set; }

        public string Address { get; set; }
    }
}
