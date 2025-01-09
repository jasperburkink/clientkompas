using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService // TODO: This service can be removed when emailmodule is ready
    {
        private const string EMAIL_ADDRESS = "ontwikkelaar@clientkompas.nl";
        private const string PASSWORD = "iaOQ6m6hsm4cK5yAEi1X";
        private const string SMTP_SERVER = "192.168.1.15";
        private const int SMTP_PORT = 1025;

        public EmailService() { }

        public async Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            await Task.Run(() =>
            {
                var smtpClient = new SmtpClient(SMTP_SERVER)
                {
                    Port = SMTP_PORT,
                    Credentials = new NetworkCredential(EMAIL_ADDRESS, PASSWORD),
                    EnableSsl = false,
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(EMAIL_ADDRESS),
                    Subject = subject,
                    Body = message
                };

                mail.To.Add(emailAddress);

                smtpClient.Send(mail);
            });
        }
    }
}
