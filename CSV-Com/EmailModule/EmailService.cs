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
        private readonly IRazorLightEngine _razorLightEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailService).Assembly)
                .UseMemoryCachingProvider()
                .Build();
        private readonly ISmtpClient _smtpClient;
        private readonly IMapper _mapper;

        public EmailService(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EmailMessageDto, EmailMessage>());
            _mapper = config.CreateMapper();
        }

        public async Task SendEmailAsync<T>(EmailMessageDto messageDto, string templateName, T model)
        {
            var message = _mapper.Map<EmailMessage>(messageDto);

            var body = await _razorLightEngine.CompileRenderAsync($"EmailModule.Templates.{templateName}.cshtml", model);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("CliëntKompas", EmailConfig.Username));

            foreach (var recipient in message.Recipients)
            {
                email.To.Add(MailboxAddress.Parse(recipient));
            }

            email.Subject = messageDto.Subject;
            email.Body = new TextPart("html") { Text = body };

            _smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await _smtpClient.ConnectAsync(EmailConfig.SmtpServer, EmailConfig.Port, false);

            if (EmailConfig.RequiresAuthentication)
            {
                await _smtpClient.AuthenticateAsync(EmailConfig.Username, EmailConfig.Password);
            }

            await _smtpClient.SendAsync(email);
            await _smtpClient.DisconnectAsync(true);
        }
    }
}
