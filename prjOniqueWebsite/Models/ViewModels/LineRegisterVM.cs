using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class LineRegisterVM
    {
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}不可為空白")]
        public string? Password { get; set; }
        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "{0}不可為空白")]
        [Compare("Password",ErrorMessage = "{0}與密碼不符")]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "手機號碼")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}不可為空白")]
        public string? Phone { get; set; }
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}不可為空白")]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public int? Citys { get; set; }
        public int? Areas { get; set; }
        public string? Address { get; set; }
        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Date)]
        public string? DateOfBirth { get; set; }
    }
}
