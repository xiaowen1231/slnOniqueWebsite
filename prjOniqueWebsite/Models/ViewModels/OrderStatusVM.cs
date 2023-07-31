namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderStatusVM
    {

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int OrderId { get; set; }
        
        public DateTime? ShippingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        

    }
}
