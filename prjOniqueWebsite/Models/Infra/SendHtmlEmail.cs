using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.AspNetCore.Hosting;
using prjOniqueWebsite.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace prjOniqueWebsite.Models.Infra
{

    public class SendHtmlEmail
    {


        public void SendOrderHtml(SendHtmlEmailContent dto, IWebHostEnvironment _enviroment, IUrlHelperFactory _urlHelperFactory,
            ControllerContext controllerContext, HttpContext httpContext)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };


            var urlHelper = _urlHelperFactory.GetUrlHelper(controllerContext);
            string orderDetailsUrl = urlHelper.Action("OrderEmailContent", "Order", new { dto.OrderId }, httpContext.Request.Scheme);


            string pathToEmail = Path.Combine(_enviroment.WebRootPath, "Template", "email.html");
            string pathToProducts = Path.Combine(_enviroment.WebRootPath, "Template", "products.html");
            string TemplateContent = "";
            using (StreamReader SourceReader = File.OpenText(pathToEmail))//讀出html內容
            {
                TemplateContent = SourceReader.ReadToEnd();
            }

            string productTable = "";
            string productTableContent = "";
            decimal prodtotalprice = 0;
            string remarkDisplay = "";
            remarkDisplay = dto.Remark == null ? "顧客未留言" : dto.Remark;

            using (StreamReader productsReader = File.OpenText(pathToProducts))//放入複數商品內容
            {
                productTable = productsReader.ReadToEnd();
            }
            foreach (var product in dto.Products)
            {
                prodtotalprice += product.SubTotal;
                productTableContent += string.Format(productTable,
                    product.ProductName,
                    product.Price.ToString("###,###,##0"),
                    product.OrderQuantity.ToString("###,###,##0"),
                    product.SizeName,
                    product.ColorName,
                    product.SubTotal.ToString("###,###,##0")
                    );
            }
            productTable = productTableContent;



            decimal shippingfee = dto.TotalPrice - prodtotalprice;
            string DisplayProdtotalPrice = prodtotalprice.ToString("###,###,##0");

            TemplateContent = string.Format(TemplateContent,

                dto.OrderId,
                dto.OrderDate,
                dto.StatusName,
                dto.MethodName,
                dto.PaymentMethodName,
                dto.Recipient,
                dto.RecipientPhone,
                dto.ShippingAddress,
                remarkDisplay,

                productTable,
                DisplayProdtotalPrice,
                shippingfee.ToString("###,###,##0"),
                dto.TotalPrice.ToString("###,###,##0"),
                orderDetailsUrl,
                   "dataEnd"
                );
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = TemplateContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(dto.Email);
            smtpClient.Send(mailMessage);

        }
    }
}
