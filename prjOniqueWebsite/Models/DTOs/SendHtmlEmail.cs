using prjOniqueWebsite.Models.Daos;
using System.Reflection;

namespace prjOniqueWebsite.Models.DTOs
{
    public class SendHtmlEmailContent
    {
        public SendHtmlEmailContent()
        {
            this.Products = new List<SendHtmlEmailProduct>();
        }
        public string Email { get; set; }
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public string StatusName { get; set; } 
        public string MethodName { get; set; }
        public string PaymentMethodName { get; set; }
        public string Recipient { get; set; }
        public string RecipientPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string Remark { get; set; }
            
       
        public decimal TotalPrice { get; set; }
        public decimal ShippingFee { get; set; }
       
        public List<SendHtmlEmailProduct> Products { get; set; }
    }
}
