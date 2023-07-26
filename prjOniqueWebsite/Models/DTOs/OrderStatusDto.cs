namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderStatusDto
    {
        public int StatusId { get; set; }
       
        public string StatusName { get; set; }
       
        public int PaymentMethodId { get; set; }

        public int OrderId { get; set; }

    }
}
