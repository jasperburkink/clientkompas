namespace Application.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordCommandDto>
    {
        public string EmailAddress { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string NewPasswordRepeat { get; set; } = null!;
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandDto>
    {
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordCommandHandler(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }

        public async Task<ResetPasswordCommandDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request.Token);
                ArgumentNullException.ThrowIfNull(request.NewPassword);

                await _resetPasswordService.ResetPassword(request.Token, request.NewPassword);

                return new ResetPasswordCommandDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResetPasswordCommandDto // TODO: Return an object with error and success false, or return result object or let API handle the exception?
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }
    }
}
