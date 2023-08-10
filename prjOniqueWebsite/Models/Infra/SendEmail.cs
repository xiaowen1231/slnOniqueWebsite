using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using NuGet.Common;
using System;
using Microsoft.AspNetCore.Http.Extensions;

namespace prjOniqueWebsite.Models.Infra
{
    public static class SendEmail
    {
        public static void SendOrderEmail(string email, int orderId, UrlHelper urlHelper, HttpContext httpContext)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            string orderDetailsUrl = urlHelper.Action("OrderEmailContent", "Order", new { orderId }, httpContext.Request.Scheme);

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = $"您的訂單處理中：<br/>{orderDetailsUrl}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);


        }
    }
}
