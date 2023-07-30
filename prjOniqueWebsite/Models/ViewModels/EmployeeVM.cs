using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class EmployeeVM
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "姓名不可空白")]
        public string EmployeeName { get; set; }
        
        public string PhotoPath { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "手機不可空白")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "信箱不可空白")]
        public string Email { get; set; }
        [Required(ErrorMessage = "密碼不可空白")]
        public string Password { get; set; }
        [Required(ErrorMessage = "身分不可空白")]

        public string EmployeeLevel { get; set; }

        public string RegisterDate { get; set; }
        [Required(ErrorMessage = "縣市不可空白")]
        public string Citys { get; set; }
        [Required(ErrorMessage = "區域不可空白")]
        public string Areas { get; set; }
        

        public string Address { get; set; }
    }
}
