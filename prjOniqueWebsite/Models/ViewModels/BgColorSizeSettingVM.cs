using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class BgColorSizeSettingVM
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        [Display(Name="顏色")]
        [Required(ErrorMessage ="請輸入顏色")]
        public string ColorName { get; set; }
        [Display(Name ="尺寸")]
        [Required(ErrorMessage ="請輸入尺寸")]
        public string SizeName { get; set; }
        public int Quantity { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }

    }
}
