using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    internal class EmailService // TODO: This service can be removed when emailmodule is ready
    {
        private const string EMAIL_ADDRESS = "ontwikkelaar@clientkompas.nl";
        private const string PASSWORD = "iaOQ6m6hsm4cK5yAEi1X";
        private const string SMTP_SERVER = "mail.mijndomein.nl";

        public EmailService() { }

        public void SendEmail(string emailAddress, string subject, string message)
        {
            var smtpClient = new SmtpClient(SMTP_SERVER)
            {
                Port = 587,
                Credentials = new NetworkCredential(EMAIL_ADDRESS, PASSWORD),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress(EMAIL_ADDRESS),
                Subject = subject,
                Body = message
            };

            mail.To.Add(emailAddress);

            smtpClient.Send(mail);
        }
    }
}
