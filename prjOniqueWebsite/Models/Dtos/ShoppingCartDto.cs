using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Dtos
{
    public class ShoppingCartDto
    {
        public Products Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int StockId { get; set; }
        public string PhotoPath { get; set; }

        public ProductColors ProductColors { get; set; }
        public ProductSizes ProductSizes { get; set; }
    }
}
