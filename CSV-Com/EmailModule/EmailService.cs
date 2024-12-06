using FluentEmail.Core;
using FluentEmail.MailKitSmtp;
using FluentEmail.Razor;

namespace EmailModule
{
    public class EmailService
    {
        private string GetTemplateFilePath(string templateName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "EmailFolder", "Templates", $"{templateName}.cshtml");
        }

        public async Task SendEmailAsync<T>(EmailMessage message, string templateName, T model)
        {
            var sender = new MailKitSender(new SmtpClientOptions
            {
                Server = EmailConfig.SmtpServer,
                Port = EmailConfig.Port,
                User = EmailConfig.Username,
                Password = EmailConfig.Password,
                UseSsl = true,
                RequiresAuthentication = true
            });

            Email.DefaultSender = sender;
            Email.DefaultRenderer = new RazorRenderer();

            var templatePath = GetTemplateFilePath(templateName);

            if (!File.Exists(templatePath))
            {
                Console.WriteLine($"Template file not found: {templatePath}");
                return;
            }

            var email = Email
                .From(EmailConfig.Username, "CliëntenKompas")
                .Subject(message.Subject)
                .UsingTemplateFromFile(templatePath, model);

            foreach (var recipient in message.To)
            {
                email.To(recipient);
            }

            var response = await email.SendAsync();

            if (response.Successful)
            {
                Console.WriteLine("E-mail succesvol verzonden!");
            }
            else
            {
                Console.WriteLine($"Fout bij het verzenden: {string.Join(", ", response.ErrorMessages)}");
            }
        }
    }
}
