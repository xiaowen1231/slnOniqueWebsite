using System.ComponentModel.DataAnnotations;
namespace prjOniqueWebsite.Models.DTOs
{
    public partial class EmployeeListDto
    {
        [Display(Name = "編號")]
        public int EmployeeId { get; set; }
        [Display(Name = "頭像")]
        public string PhotoPath { get; set; }
        [Display(Name = "員工姓名")]
        public string EmployeeName { get; set; }
        [Display(Name = "生日")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "性別")]
        public string Gender { get; set; }
        [Display(Name = "電話")]
        public string Phone { get; set; }
        [Display(Name = "信箱")]
        public string Email { get; set; }
        [Display(Name = "身分")]
        public string EmployeeLevelName { get; set; }


    }
}
