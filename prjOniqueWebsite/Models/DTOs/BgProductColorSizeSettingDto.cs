using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.DTOs
{
    public class BgProductColorSizeSettingDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ProductSizes> ProductSizes { get; set; }
        public List<ProductColors> ProductColors { get; set; }


    }
}
