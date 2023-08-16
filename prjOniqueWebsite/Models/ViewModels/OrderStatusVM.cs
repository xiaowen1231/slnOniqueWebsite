namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderStatusVM
    {

        public int StatusId { get; set; }

        public string StatusName { get; set; }
        public string formerStatusName { get; set; }
        public string? OrderId { get; set; }
        
        public DateTime? ShippingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        
        public int OrderQuantity { get; set; }
        public int StockId { get; set; }
    }
}
