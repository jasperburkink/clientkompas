using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<string, DateTime> s_emailSendTimes = new();
        private readonly List<(MimeMessage, DateTime)> _mailMessages = new();

        public EmailService(IMapper mapper)
        {
            _mapper = mapper;
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailService).Assembly)
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

                foreach (var recipient in message.Recipients)
                {
                    if (IsRateLimited(recipient))
                    {
                        Debug.WriteLine($"Rate limit exceeded for {recipient}");
                        continue;
                    }

                    email.To.Add(MailboxAddress.Parse(recipient));
                    UpdateRateLimit(recipient);
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

                if (IsDuplicateEmail(email))
                {
                    Debug.WriteLine("Duplicate email detected");
                    return;
                }

                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                _mailMessages.Add((email, DateTime.Now));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private bool IsDuplicateEmail(MimeMessage email)
        {
            foreach (var (sentmail, _) in _mailMessages)
            {
                if (email.Subject == sentmail.Subject && email.Body == sentmail.Body)
                {
                    Debug.WriteLine("Duplicate email detected");
                    return true;
                }
            }
            return false;
        }

        private bool IsRateLimited(string recipient)
        {
            if (s_emailSendTimes.TryGetValue(recipient, out var lastSent))
            {
                var timeSinceLastSent = DateTime.UtcNow - lastSent;
                if (timeSinceLastSent < TimeSpan.FromMinutes(1))
                {
                    _mailMessages.Clear();
                    return true;
                }
            }
            return false;
        }

        private void UpdateRateLimit(string recipient)
        {
            s_emailSendTimes[recipient] = DateTime.UtcNow;
        }
    }
}
