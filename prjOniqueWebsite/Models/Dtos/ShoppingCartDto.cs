using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Dtos
{
    public class ShoppingCartDto
    {
        public Products Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ProductStockDetails ProductStock { get; set; }
    }
}
