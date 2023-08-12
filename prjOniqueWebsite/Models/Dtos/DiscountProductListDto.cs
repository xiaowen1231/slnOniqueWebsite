namespace prjOniqueWebsite.Models.Dtos
{
    public class DiscountProductListDto
    {
        public int ProductId { get; set; }
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountMethod { get; set; }
        public string DiscountPrice
        {
            get
            {
                return (this.Price * this.DiscountMethod).ToString("0");
            }
        }
    }
}
