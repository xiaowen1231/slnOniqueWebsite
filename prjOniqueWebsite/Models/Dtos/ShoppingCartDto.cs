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

        public decimal? DiscountMethod { get; set; }
        public decimal? DiscountPrice
        {
            get
            {
                if (DiscountMethod!.HasValue)
                {
                    return Math.Round((decimal)(this.Product.Price * this.DiscountMethod));
                }
                else { return null; }
            }

        }
    }
}
