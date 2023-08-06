namespace prjOniqueWebsite.Models.DTOs
{
    public class ProductsListDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
        public DateTime AddedTime { get; set; }
        public int SubQuantity { get; set; }
        public string catagoryName { get; set; }
    }
}
