using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using FluentEmail.Core;
using FluentEmail.MailKitSmtp;
using RazorLight;
using System.Diagnostics;

namespace EmailModule
{
    public class EmailService : IEmailService
    {
        private readonly IMapper _mapper;
        private readonly IRazorLightEngine _razorLightEngine;

        public EmailService(IMapper mapper)
        {
            _mapper = mapper;
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailService).Assembly) // Laad sjablonen uit de embedded resources
                .UseMemoryCachingProvider()
                .Build();
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
                    UseSsl = EmailConfig.Port == 465,
                    RequiresAuthentication = true
                });


                Email.DefaultSender = sender;

                var template = await _razorLightEngine.CompileRenderAsync($"EmailModule.Templates.{templateName}.cshtml", model);

                var email = Email
                    .From(EmailConfig.Username, "CliëntenKompas")
                    .Subject(message.Subject)
                    .UsingTemplate(template, model);

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
