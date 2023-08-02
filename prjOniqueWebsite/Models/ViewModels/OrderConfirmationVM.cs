using prjOniqueWebsite.Models.EFModels;
using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderConfirmationVM
    {
        public int MethodId { get; set; }
        public int PaymentMethodId { get; set; }
        public int MemberId { get; set; }
        public string Recipient { get; set; }
        public string RecipientPhone { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
    }
}
