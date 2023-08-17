namespace prjOniqueWebsite.Models.DTOs
{
    public class SendHtmlEmailProduct
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int OrderQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public decimal SubTotal
        {
            get { return this.Price * this.OrderQuantity; }
           
        }

    }
}
