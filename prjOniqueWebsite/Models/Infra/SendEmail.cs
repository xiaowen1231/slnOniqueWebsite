using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using NuGet.Common;
using System;
using Microsoft.AspNetCore.Http.Extensions;
using prjOniqueWebsite.Models.Dtos;
using Microsoft.Extensions.Hosting;

namespace prjOniqueWebsite.Models.Infra
{
    public  class SendEmail
    {
       
        public void SendMail(SendMailDto dto,IUrlHelperFactory _urlHelperFactory ,ControllerContext controllerContext,HttpContext httpContext)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };


            var urlHelper = _urlHelperFactory.GetUrlHelper(controllerContext);


            string orderDetailsUrl = urlHelper.Action("OrderEmailContent", "Order", new { dto.OrderId }, httpContext.Request.Scheme);


            string htmlStr =
                $@"
    <div style=""font-size:24px"">
        <div>親愛的{dto.Name}您好!</div>
        <div>您的訂單 編號:{dto.OrderId}</div>
        <div>您的訂單狀態目前已更新為:{dto.StatusNow}</div>
        <div><a href=""{orderDetailsUrl}"">點擊查看訂單狀態</a></div>
    </div>";
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = htmlStr,
                IsBodyHtml = true
            };

            mailMessage.To.Add(dto.Email);
            smtpClient.Send(mailMessage);

        }
    }
}
