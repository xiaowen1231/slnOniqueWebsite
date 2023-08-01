using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public int MethodId { get; set; }
        public int PaymentMethodId { get; set; }
        public int MemberId { get; set; }
        public string Recipient { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
}
