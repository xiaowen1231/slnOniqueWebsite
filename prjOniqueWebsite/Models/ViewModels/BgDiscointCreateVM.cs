using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class BgDiscointCreateVM
    {
        public int? Id { get; set; }
        [Display(Name = "活動名稱")]
        [Required(ErrorMessage = "活動名稱不可為空白")]
        public string? Title { get; set; }

        [Display(Name = "活動說明")]
        [Required(ErrorMessage = "活動說明不可為空白")]
        public string? Description { get; set; }

        [Display(Name = "折扣方式")]
        [Required(ErrorMessage = "折扣不可為空白")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Range(0.1,0.9,ErrorMessage ="必須為小數0.1至0.9")]
        public decimal? DiscountMethod { get; set; }

        [Display(Name = "生效日期")]
        [Required(ErrorMessage = "生效日期不可為空白")]
        public DateTime? BeginDate { get; set; } = DateTime.Now;  

        [Display(Name = "結束日期")]
        [Required(ErrorMessage = "結束日期不可為空白")]
        public DateTime? EndDate { get; set; } = DateTime.Now.AddDays(7); 

        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
