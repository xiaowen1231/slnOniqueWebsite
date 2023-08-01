using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjOniqueWebsite.Models.ViewModels
{
    public partial class EmployeeVM
    {

        public int EmployeeId { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名不可空白")]
        public string EmployeeName { get; set; }
        
        public string? PhotoPath { get; set; }
        public string? DateOfBirth { get; set; }

        [Display(Name = "性別")]
        [Required(ErrorMessage = "性別不可空白")]
        public bool Gender { get; set; }

        [Display(Name = "電話")]
        [Required(ErrorMessage = "手機不可空白")]
        public string Phone { get; set; }

        [Display(Name = "信箱")]
        [Required(ErrorMessage = "信箱不可空白")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼不可空白")]
        public string Password { get; set; }

        [Display(Name = "身分")]
        [Required(ErrorMessage = "身分不可空白")]

        public string EmployeeLevel { get; set; }

        public string RegisterDate { get; set; }

        [Display(Name = "縣市")]
        [Required(ErrorMessage = "縣市不可空白")]
        public string Citys { get; set; }

        [Display(Name = "區域")]
        [Required(ErrorMessage = "區域不可空白")]
        public string Areas { get; set; }
        

        public string? Address { get; set; }
    }
}
