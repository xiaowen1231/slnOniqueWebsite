namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderProductsListDto
    {
        public string ProductName { get; set; }
        public int OrderQuantity { get; set; }
        public decimal Price { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string PhotoPath { get;set; }
    }
}
