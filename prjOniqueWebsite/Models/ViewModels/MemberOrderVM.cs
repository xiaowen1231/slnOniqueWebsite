namespace prjOniqueWebsite.Models.ViewModels
{
    public class MemberOrderVM
    {
        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal Price { get; set; }
    }
}
