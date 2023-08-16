namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderListDto
    {
        public string StatusName { get; set; }
        public string? OrderId { get; set; }
        public string Name { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public string PaymentMethodName { get; set; }
        public string PhotoPath { get; set; }
    }
}
