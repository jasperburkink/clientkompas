using System.Diagnostics;
using System.Net.Mail;

namespace AuthenticationPoC.Helpers
{
    public class EmailHelper
    {
        // other methods

        public bool SendEmailTwoFactorCode(string userEmail, string code)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("jasper.burkink@specializedbrainsict.nl");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Two Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                
                EnableSsl = true,
                UseDefaultCredentials = false
            })
            {
                client.Credentials = new System.Net.NetworkCredential("jasper.sbict@gmail.com", "hdeazksswomiiaqd");
                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            
            return false;
        }
    }
}
