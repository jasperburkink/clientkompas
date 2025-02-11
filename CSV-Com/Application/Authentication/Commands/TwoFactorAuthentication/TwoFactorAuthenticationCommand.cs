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

    public class TwoFactorAuthenticationCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, ITokenService tokenService, IResourceMessageProvider resourceMessageProvider) : IRequestHandler<TwoFactorAuthenticationCommand, TwoFactorAuthenticationCommandDto>
    {
        private const string RESOURCE_KEY_USERNOTLOGGEDIN = "UserNotLoggedIn";
        private const string RESOURCE_KEY_INVALIDTOKEN = "InvalidToken";

        public async Task<TwoFactorAuthenticationCommandDto> Handle(TwoFactorAuthenticationCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserAsync(request.UserId);
            var twoFactorPendingToken = await tokenService.GetTokenAsync(request.TwoFactorPendingToken, nameof(request.TwoFactorPendingToken));

            if (!IsUserLoggedIn(request, user, twoFactorPendingToken))
            {
                throw new InvalidLoginException(resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), RESOURCE_KEY_USERNOTLOGGEDIN));
            }

            var loggedInResult = await identityService.Login2FAAsync(request.UserId, request.Token);

            if (IsInvalidLogin(loggedInResult) || !await tokenService.ValidateTokenAsync(user.Id, twoFactorPendingToken.Value, nameof(request.TwoFactorPendingToken)))
            {
                throw new InvalidLoginException(resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), RESOURCE_KEY_INVALIDTOKEN));
            }

            var bearerToken = await bearerTokenService.GenerateBearerTokenAsync(loggedInResult.User, loggedInResult.Roles); // UserInfo & roles are processed inside the bearertoken

            var refreshToken = await tokenService.GenerateTokenAsync(loggedInResult.User, nameof(TwoFactorAuthenticationCommandDto.RefreshToken));

            return new TwoFactorAuthenticationCommandDto
            {
                Success = true,
                BearerToken = bearerToken,
                RefreshToken = refreshToken
            };
        }

        private bool IsUserLoggedIn(TwoFactorAuthenticationCommand request, IAuthenticationUser user, IAuthenticationToken? twoFactorPendingToken)
        {
            return user != null
                && twoFactorPendingToken != null
                && twoFactorPendingToken.UserId == request.UserId;
        }

        private static bool IsInvalidLogin(LoggedInResult loggedInUser)
        {
            return !loggedInUser.Succeeded || loggedInUser.User == null || loggedInUser.Roles == null;
        }
    }
}
