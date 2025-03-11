using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.RequestResetPassword
{
    public record RequestResetPasswordCommand : IRequest<RequestResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;
    }

    public class RequestResetPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<RequestResetPasswordCommand, RequestResetPasswordCommandDto>
    {
        public async Task<RequestResetPasswordCommandDto> Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(request.EmailAddress);

                var result = await identityService.SendResetPasswordEmailAsync(request.EmailAddress);

                return new RequestResetPasswordCommandDto
                {
                    Success = result.Succeeded
                };
            }
            catch (Exception ex)
            {
                return new RequestResetPasswordCommandDto
                {
                    Success = false,
                    Errors = [ex.Message]
                };
            }
        }
    }
}
