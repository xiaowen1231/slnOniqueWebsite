using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class EmployeeEditVM
    {
        public int EmployeeId { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string EmployeeName { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string? Email { get; set; }
        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }
        public string? Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string? Address { get; set; }
        public string? EmployeeLevel { get; set; }
        public string? RegisterDate { get; set; }

        public string? DateOfBirth { get; set; }
    }
}
