using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace prjOniqueWebsite.Models.ViewModels
{
    public partial class EmployeeVM
    {

        public int EmployeeId { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        public string EmployeeName { get; set; }
        
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
        [Display(Name = "生日")]
        [Required(ErrorMessage = "請輸入生日日期")]
        public string? DateOfBirth { get; set; }

        [Required(ErrorMessage = "請選擇性別")]
        public string Gender { get; set; }

        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [Display(Name = "信箱")]
        [Required(ErrorMessage = "請輸入電子信箱")]
        [RegularExpression(@"^[^\s!#$%^&*()_+{}\[\]:;<>,?~\\/]+$", ErrorMessage = "請輸入不包含特殊符號的商品名稱")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
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
