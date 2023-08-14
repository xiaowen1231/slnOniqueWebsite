using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class FMemberPasswordVM
    {
        public int MemberId { get; set; }
        public string? Password { get; set; }
        [Display(Name = "舊密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string? OldPassword { get; set; }
        [Display(Name = "新密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string? NewPassword { get; set; }
        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "尚未{0}!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "{0}必須與新密碼一致")]
        public string? ConfirmPassword { get; set; }
    }
}
