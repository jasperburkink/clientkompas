using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.ResetPassword
{
    public record ResetPasswordCommand : IRequest<ResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string NewPasswordRepeat { get; set; } = null!;
    }

    public class ResetPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandDto>
    {
        public async Task<ResetPasswordCommandDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request.EmailAddress);
                ArgumentNullException.ThrowIfNull(request.Token);
                ArgumentNullException.ThrowIfNull(request.NewPassword);

                var result = await identityService.ResetPasswordAsync(request.EmailAddress, request.Token, request.NewPassword);

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
                    Errors = [ex.Message]
                };
            }
        }
    }
}
