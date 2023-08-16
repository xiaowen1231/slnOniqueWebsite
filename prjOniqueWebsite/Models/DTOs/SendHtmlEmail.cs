using prjOniqueWebsite.Models.Daos;
using System.Reflection;

namespace prjOniqueWebsite.Models.DTOs
{
    public class SendHtmlEmail
    {

        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusName { get; set; } 
        public string MethodName { get; set; }
        public string PaymentMethodName { get; set; }
        public string Recipient { get; set; }
        public string RecipientPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string DisplayRemark { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int OrderQuantity { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ShippingFee { get; set; }

    }
}
