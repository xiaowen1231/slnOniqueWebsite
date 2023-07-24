namespace prjOniqueWebsite.Models.DTOs
{
    public class BgEditProductDto
    {
        public string ProductName { get; set; }
        public string PhotoPath { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
       public int Discont { get; set; }
        public string SupplierName { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ShelfTime { get; set; }

        public string Description { get; set; }
    }
}
