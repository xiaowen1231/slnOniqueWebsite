using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class FMemberVM
    {
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "尚未{0}!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="{0}必須與密碼一致")]
        public string ComfirmPassword { get; set; }
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Citys { get; set; }
        public string Areas { get; set; }
        public string? Address { get; set; }
        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
    }
}
