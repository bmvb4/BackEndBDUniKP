using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BackEndBDAPP.Utils
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("admin@beleaf.social");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("admin@beleaf.social", "beLeaf1999uni");
            client.Host = "smtp.titan.email";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            client.Port = 465;
     

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
