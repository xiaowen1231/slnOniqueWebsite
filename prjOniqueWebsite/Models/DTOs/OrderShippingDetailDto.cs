namespace prjOniqueWebsite.Models.DTOs
{
    public class OrderShippingDetailDto
    {
        public string? OrderId { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }
        public string MethodName { get; set; }
        public string StatusName { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public string Recipient { get; set; }
        public string Remark { get; set; }
        public string RecipientPhone { get; set; }
        public string Phone { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippingDate { get;set; }
        public DateTime? CompletionDate { get; set; }
 
    }
}
