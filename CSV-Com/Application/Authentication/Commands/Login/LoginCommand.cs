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
        private readonly IBearerTokenService _bearerTokenService;

        public LoginCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
        }

        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isUserLoggedIn = await _identityService.LoginAsync(request.UserName, request.Password);

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync();

            return new LoginCommandDto
            {
                Success = isUserLoggedIn,
                BearerToken = bearerToken
            };
        }
    }
}
