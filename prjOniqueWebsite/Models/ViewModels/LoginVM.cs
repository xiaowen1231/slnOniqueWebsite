using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "電子郵件")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
