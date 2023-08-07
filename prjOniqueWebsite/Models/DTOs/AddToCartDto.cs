namespace prjOniqueWebsite.Models.DTOs
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PhotoPath { get; set; }
        public decimal Price { get; set; }

    }
}
