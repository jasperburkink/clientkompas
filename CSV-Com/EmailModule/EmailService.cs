using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using FluentEmail.Core;
using FluentEmail.MailKitSmtp;
using FluentEmail.Razor;

namespace EmailModule
{
    public class EmailService : IEmailService
    {
        private readonly IConfigurationProvider _configuration;

        private readonly IMapper _mapper;

        public EmailService()
        {
            _configuration = new MapperConfiguration(config =>

            config.AddProfile<MappingProfile>());
            _mapper = _configuration.CreateMapper();

        }

        private string GetTemplateFilePath(string templateName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Templates", $"{templateName}.cshtml");
        }

        public async Task SendEmailAsync<T>(EmailMessageDto messageDto, string templateName, T model)
        {
            try
            {
                var message = _mapper.Map<EmailMessage>(messageDto);

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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
