using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BackEndBDAPP.Utils
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink, string Username)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("admin@beleaf.me");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "https://beleaf.me/Email/"+Username+"/"+confirmationLink;

            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("admin@beleaf.me", "beLeaf1999uni");
            client.Host = "mail.privateemail.com";
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Port = 587;


            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
