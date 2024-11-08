using Application.Common.Exceptions;
using Application.Common.Interfaces;
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
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public LoginCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, IRefreshTokenService refreshTokenService, IResourceMessageProvider resourceMessageProvider)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
            _refreshTokenService = refreshTokenService;
            _resourceMessageProvider = resourceMessageProvider;
        }

        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _identityService.LoginAsync(request.UserName!, request.Password!);

            if (!loggedInUser.Succeeded || loggedInUser.User == null || loggedInUser.Roles == null)
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), "InvalidLogin"));
            }

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(loggedInUser.User, loggedInUser.Roles); // UserInfo & roles are processed inside the bearertoken

            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(loggedInUser.User);

            return new LoginCommandDto
            {
                Success = true,
                BearerToken = bearerToken,
                RefreshToken = refreshToken
            };
        }
    }
}
