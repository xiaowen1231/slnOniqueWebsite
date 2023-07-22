using prjOniqueWebsite.Models.EFModels;
using System.Drawing;

namespace prjOniqueWebsite.Models.Dtos
{
    public class ProductDetailDto
    {
        public ProductDetailDto()
        {
            PSD_List = new List<ProductStockDetails>();
            Color = new List<ProductColors>();
            Size = new List<ProductSizes>();
        }
        public List<ProductStockDetails> PSD_List { get; set; }
        public Products products { get; set; }
        public List<ProductColors> Color { get; set; }
        public List<ProductSizes> Size { get; set; }
    }
}
