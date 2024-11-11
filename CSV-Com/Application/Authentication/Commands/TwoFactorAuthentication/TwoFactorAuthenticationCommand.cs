using Application.Authentication.Commands.Login;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.TwoFactorAuthentication
{
    public record TwoFactorAuthenticationCommand : IRequest<TwoFactorAuthenticationCommandDto>
    {
        public string UserId { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

    public class TwoFactorAuthenticationCommandHandler : IRequestHandler<TwoFactorAuthenticationCommand, TwoFactorAuthenticationCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public TwoFactorAuthenticationCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, IRefreshTokenService refreshTokenService, IResourceMessageProvider resourceMessageProvider)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
            _refreshTokenService = refreshTokenService;
            _resourceMessageProvider = resourceMessageProvider;
        }

        public async Task<TwoFactorAuthenticationCommandDto> Handle(TwoFactorAuthenticationCommand request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _identityService.Login2FAAsync(request.UserId, request.Token);

            if (!loggedInUser.Succeeded || loggedInUser.User == null || loggedInUser.Roles == null)
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), "InvalidToken"));
            }

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(loggedInUser.User, loggedInUser.Roles); // UserInfo & roles are processed inside the bearertoken

            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(loggedInUser.User);

            return new TwoFactorAuthenticationCommandDto
            {
                Success = true,
                BearerToken = bearerToken,
                RefreshToken = refreshToken
            };
        }
    }
}
