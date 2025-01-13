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

        public readonly List<(EmailMessage, DateTime)> MailMessagesSent = new();
        public readonly List<(EmailMessage, DateTime, string)> FailedMailMessagesSent = new();
        private readonly System.Timers.Timer _timer = new(60000);


        public EmailService(IMapper mapper)
        {
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
            _timer.Start();
            _mapper = mapper;
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailService).Assembly)
                .UseMemoryCachingProvider()
                .Build();
        }

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
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

                if (IsDuplicateEmail(messageDto))
                {
                    Debug.WriteLine("Duplicate email detected");
                    return;
                }

                await client.SendAsync(email);
                await client.DisconnectAsync(true);

                MailMessagesSent.Add((message, DateTime.Now));


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WriteLog(EmailMessage email, string result, string recipient = "", string error = "")
        {
            var logPath = "EmailModule.Logging";

            using var writer = new StreamWriter(logPath, true);
            {
                writer.WriteLine($"{DateTime.Now} | {email.Id} | {EmailConfig.Username} | {recipient} | {email.Subject} | {result} | {error}");
            }
        }



        private bool IsDuplicateEmail(EmailMessageDto email)
        {
            foreach (var (sentmail, _) in MailMessagesSent)
            {
                if (sentmail.IsDuplicateEmail(email))
                {
                    Debug.WriteLine("Duplicate email detected");
                    var failedEmail = _mapper.Map<EmailMessage>(email);
                    FailedMailMessagesSent.Add((failedEmail, DateTime.Now, "Duplicate Email"));
                    return true;
                }
            }
            return false;
        }

        private bool IsRateLimited(string recipient)
        {
            if (s_emailSendTimes.TryGetValue(recipient, out var lastSent))
            {
                // TO DO: Periodiek logging en dan worden de lijsten leeg gemaakt en weg gescheven naar de log
                var timeSinceLastSent = DateTime.UtcNow - lastSent;
                if (timeSinceLastSent < TimeSpan.FromMinutes(1))
                {
                    //MailMessagesSent.Clear();
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
