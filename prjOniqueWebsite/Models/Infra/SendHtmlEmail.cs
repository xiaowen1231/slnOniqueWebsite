using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.AspNetCore.Hosting;

namespace prjOniqueWebsite.Models.Infra
{
    
    public class SendHtmlEmail
    {
        

        public void SendOrderHtml(IWebHostEnvironment _enviroment)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            string pathToFile = Path.Combine(_enviroment.WebRootPath, "Template", "email.html");

            string TemplateContent = "";
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))//讀出html內容
            {
                TemplateContent = SourceReader.ReadToEnd();
            }
            TemplateContent = string.Format(TemplateContent,

                "fakedata1",
                "fakedata2",
                "fakedata3",
                "fakedata4",
                "fakedata5",
                "fakedata6",
                "fakedata7",
                "fakedata8",
                "fakedata9",
                "fakedata10",
                "fakedata11",
                "fakedata12",
                "fakedata13",
                "fakedata14",
                "fakedata15",
                "fakedata16",
                "fakedata17",
                "fakedata18",
                "fakedata19"


                );
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = TemplateContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(/*Email*/"");
            smtpClient.Send(mailMessage);

        }
    }
}
