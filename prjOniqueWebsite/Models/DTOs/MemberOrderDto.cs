namespace prjOniqueWebsite.Models.DTOs
{
    public class MemberOrderDto
    {

        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public DateTime  OrderDate { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
