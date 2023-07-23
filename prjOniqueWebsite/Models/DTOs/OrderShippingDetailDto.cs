namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderShippingDetailDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public string MethodName { get; set; }
        public string StatusName { get; set; }
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippingDate { get;set; }
        public DateTime? CompletionDate { get; set; }
 
    }
}
