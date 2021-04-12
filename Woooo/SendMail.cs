using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Woooo
{
    public class SendMail
    {
        public static bool SendEmail(string body, string subject, string[] to)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ///////////////SMTP CONFIGURATIONS
            //SmtpClient smtpClient = new SmtpClient("smtp.ipage.com", 587);
            SmtpClient smtpClient = new SmtpClient("smtp.ipage.com", 587);
            NetworkCredential basicCredential =
                new NetworkCredential("noreply@woooo.world", "NoReply@W(90)");
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("noreply@woooo.world", "Woooo");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = basicCredential;
            message.From = fromAddress;
            message.Subject = subject;
            //Set IsBodyHtml to true means you can send HTML email.
            message.IsBodyHtml = true;
            message.Body = body;

            foreach (var item in to)
            {
                message.To.Add(item);
            }


            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //Error, could not send the message
                // Response.Write(ex.Message);
            }
            //////////////////////////////////////////






        }
    }
}