using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;

namespace Application.Authentication.Commands.ResetPassword
{
    public record ResetPasswordCommand : IRequest<ResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string NewPasswordRepeat { get; set; } = null!;
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;

        public ResetPasswordCommandHandler(IIdentityService identityService, IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<ResetPasswordCommandDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request.EmailAddress);
                ArgumentNullException.ThrowIfNull(request.Token);
                ArgumentNullException.ThrowIfNull(request.NewPassword);

                var result = await _identityService.ResetPasswordAsync(request.EmailAddress, request.Token, request.NewPassword);

                EmailMessageDto message = new()
                {
                    Id = Guid.NewGuid(),
                    Subject = "Uw wachtwoord is veranderd",
                    Recipients = new List<string> { request.EmailAddress }
                };

                if (result.Succeeded)
                {
                    await _emailService.SendEmailAsync(message, "PasswordResetConfirmation", new { });
                }

                return new ResetPasswordCommandDto
                {
                    Success = result.Succeeded,
                    Errors = result.Errors
                };
            }
            catch (Exception ex)
            {
                return new ResetPasswordCommandDto
                {
                    Success = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
