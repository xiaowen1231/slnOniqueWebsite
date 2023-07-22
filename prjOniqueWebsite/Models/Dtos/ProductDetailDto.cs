namespace prjOniqueWebsite.Models.Dtos
{
    public class ProductDetailDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<string> Color { get; set; }
        public List<string> Size { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
    }
}
