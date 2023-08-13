using prjOniqueWebsite.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class BgProductsVM
    {
        public int ProductId { get; set; }

        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "商品名稱不可空白")]
        [RegularExpression(@"^[^\s!@#$%^&*()_+{}\[\]:;<>,.?~\\/]+$", ErrorMessage = "請輸入不包含特殊符號的商品名稱")]
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }

        [Display(Name = "商品價格")]
        [Required(ErrorMessage = "商品價格不可空白")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為正整數")]
        [Range(0,99999999999,ErrorMessage ="必須為正整數")]
        public decimal? Price { get; set; }
        public string? Description { get; set; }

        [Display(Name = "上架時間")]
        [Required(ErrorMessage = "上架時間不可空白")]
        public DateTime? AddedTime { get; set; }

        [Display(Name = "下架時間")]
        [Required(ErrorMessage = "下架時間不可空白")]
        public DateTime? ShelfTime { get; set; }
        public int SupplierId { get; set; }
        public int? DiscountId { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
