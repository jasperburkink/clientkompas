using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.RequestResetPassword
{
    public record RequestResetPasswordCommand : IRequest<RequestResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;
    }

    public class RequestResetPasswordCommandHandler : IRequestHandler<RequestResetPasswordCommand, RequestResetPasswordCommandDto>
    {
        private readonly IIdentityService _identityService;

        public RequestResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RequestResetPasswordCommandDto> Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(request.EmailAddress);

                var result = await _identityService.SendResetPasswordEmailAsync(request.EmailAddress);

                return new RequestResetPasswordCommandDto
                {
                    Success = result.Succeeded,
                    Errors = result.Errors
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
