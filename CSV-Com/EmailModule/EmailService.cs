using System.Diagnostics;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MailKit.Net.Smtp;
using MimeKit;
using RazorLight;

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

                var body = await _razorLightEngine.CompileRenderAsync($"EmailModule.Templates.{templateName}.cshtml", model);

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("CliëntKompas", EmailConfig.Username));

                foreach (var recipient in message.To)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }

                email.Subject = messageDto.Subject;
                email.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(EmailConfig.SmtpServer, EmailConfig.Port, false);

                if (EmailConfig.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(EmailConfig.Username, EmailConfig.Password);
                }

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
