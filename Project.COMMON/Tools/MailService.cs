using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MVCMailService.Tools
{
    public static class MailService
    {
        public static void send(string receiver, string password = "DarknessCoffeeBK85", string body="Test mesajı",string subject="Email Testi",string sender= "DarknessCoffeeBK@gmail.com")
        {
            MailAddress senderEmail = new MailAddress(sender);
            MailAddress receiverEmail = new MailAddress(receiver);

            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials=new NetworkCredential(senderEmail.Address,password)
            };
            using (MailMessage massage = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body=body,

            })
            {
                smtp.Send(massage);
            }
        }
    }
}