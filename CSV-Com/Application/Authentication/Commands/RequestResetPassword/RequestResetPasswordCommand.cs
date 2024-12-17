using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;

namespace Application.Authentication.Commands.RequestResetPassword
{
    public record RequestResetPasswordCommand : IRequest<RequestResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;
    }

    public class RequestResetPasswordCommandHandler : IRequestHandler<RequestResetPasswordCommand, RequestResetPasswordCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;
        private const string WEBAPP_URL = "http://localhost:3000"; // TODO: Move this url to the appsettings or get the url from constants?


        public RequestResetPasswordCommandHandler(IIdentityService identityService, IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<RequestResetPasswordCommandDto> Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(request.EmailAddress);

                var token = await _identityService.GetResetPasswordEmailToken(request.EmailAddress);

                if (token == string.Empty)
                {
                    return new RequestResetPasswordCommandDto
                    {
                        Success = true
                    };
                }

                var encodedToken = Uri.EscapeDataString(token);

                var link = new Uri($"{WEBAPP_URL}/reset-password/{request.EmailAddress}/{encodedToken}");

                // Send via email
                EmailMessageDto message = new()
                {
                    Subject = "Herstel Wachtwoord",
                    Recipients = new List<string> { request.EmailAddress }
                };

                await _emailService.SendEmailAsync(message, "PasswordForgotten", new { ResetLink = link, User = request.EmailAddress });

                return new RequestResetPasswordCommandDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new RequestResetPasswordCommandDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
