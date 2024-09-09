using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.Login
{
    public record LoginCommand : IRequest<LoginCommandDto>
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandDto>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isUserLoggedIn = await _identityService.LoginAsync(request.UserName, request.Password);

            return new LoginCommandDto
            {
                Success = isUserLoggedIn
            };
        }
    }
}
