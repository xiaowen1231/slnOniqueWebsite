using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using NuGet.Common;

namespace prjOniqueWebsite.Controllers
{
    public class EmailController : Controller
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public EmailController(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HttpGet]
        public IActionResult SendOrderEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOrderEmail(string email,int orderId)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };


            var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);


            string orderDetailsUrl = urlHelper.Action("OrderEmailContent", "Order", new { orderId }, HttpContext.Request.Scheme);

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = $"您的訂單處理中：<br/>{orderDetailsUrl}",
                IsBodyHtml = true
            };
           
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);

            ViewBag.Message = "郵件已發送!";

            return View();
        }
        
    }
}

