namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderStatusDto
    {
        public int StatusId { get; set; }
       
        public string StatusName { get; set; }
       
        public string PaymentMethodName { get; set; }

        public string? OrderId { get; set; }
        public DateTime OrderDate { get; set; }


    }
}
