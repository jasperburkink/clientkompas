using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Domain;

namespace Application.Authentication.Commands.TwoFactorAuthentication
{
    public record TwoFactorAuthenticationCommand : IRequest<TwoFactorAuthenticationCommandDto>
    {
        public string UserId { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string TwoFactorPendingToken { get; set; } = null!;
    }

    public class TwoFactorAuthenticationCommandHandler : IRequestHandler<TwoFactorAuthenticationCommand, TwoFactorAuthenticationCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IBearerTokenService _bearerTokenService;
        private readonly ITokenService _tokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public TwoFactorAuthenticationCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, ITokenService tokenService, IResourceMessageProvider resourceMessageProvider)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
            _tokenService = tokenService;
            _resourceMessageProvider = resourceMessageProvider;
        }

        public async Task<TwoFactorAuthenticationCommandDto> Handle(TwoFactorAuthenticationCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserAsync(request.UserId);
            var twoFactorPendingToken = await _tokenService.GetTokenAsync(request.TwoFactorPendingToken, "TwoFactorPendingToken");

            if (!IsUserLoggedIn(request, user, twoFactorPendingToken))
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), "UserNotLoggedIn"));
            }

            var loggedInResult = await _identityService.Login2FAAsync(request.UserId, request.Token);

            if (IsInvalidLogin(loggedInResult))
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), "InvalidToken"));
            }

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(loggedInResult.User, loggedInResult.Roles); // UserInfo & roles are processed inside the bearertoken

            var refreshToken = await _tokenService.GenerateTokenAsync(loggedInResult.User, "RefreshToken");

            return new TwoFactorAuthenticationCommandDto
            {
                Success = true,
                BearerToken = bearerToken,
                RefreshToken = refreshToken
            };
        }

        private static bool IsUserLoggedIn(TwoFactorAuthenticationCommand request, AuthenticationUser user, IToken? twoFactorPendingToken)
        {
            return user != null && twoFactorPendingToken != null && twoFactorPendingToken.UserId == request.UserId;
        }

        private static bool IsInvalidLogin(LoggedInResult loggedInUser)
        {
            return !loggedInUser.Succeeded || loggedInUser.User == null || loggedInUser.Roles == null;
        }
    }
}
