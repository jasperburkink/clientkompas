
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.TwoFactorAuthentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;

namespace Application.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public record ResendTwoFactorAuthenticationTokenCommand : IRequest<ResendTwoFactorAuthenticationTokenCommandDto>
    {
        public string UserId { get; set; } = null!;
        public string TwoFactorPendingToken { get; set; } = null!;
    }

    public class ResendTwoFactorAuthenticationTokenCommandHandler(IIdentityService identityService, ITokenService tokenService, IResourceMessageProvider resourceMessageProvider, IEmailService emailService) : IRequestHandler<ResendTwoFactorAuthenticationTokenCommand, ResendTwoFactorAuthenticationTokenCommandDto>
    {
        public async Task<ResendTwoFactorAuthenticationTokenCommandDto> Handle(ResendTwoFactorAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUserAsync(request.UserId);
            var currentTwoFactorPendingToken = await tokenService.GetTokenAsync(request.TwoFactorPendingToken, nameof(request.TwoFactorPendingToken));

            if (!await IsUserLoggedIn(request, user, currentTwoFactorPendingToken))
            {
                throw new InvalidLoginException(resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), AuthenticationCommandContants.RESOURCE_KEY_USERNOTLOGGEDIN));
            }

            // Token value that user needs to enter
            var twoFactorAuthenticationTokenValue = await identityService.Get2FATokenAsync(user.Id);

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new NotFoundException(resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), AuthenticationCommandContants.RESOURCE_KEY_NOEMAILADDRESS));
            }

            // Security token for checking loginstatus user
            var twoFactorPendingTokenValue = await tokenService.GenerateTokenAsync(user, nameof(request.TwoFactorPendingToken));

            // Send the token via email
            await emailService.SendEmailAsync(user.Email, "Two-factor authentication token", twoFactorAuthenticationTokenValue);

            return new ResendTwoFactorAuthenticationTokenCommandDto
            {
                Success = true,
                UserId = user.Id,
                TwoFactorPendingToken = twoFactorPendingTokenValue,
                ExpiresAt = DateTime.UtcNow.Add(TwoFactorPendingTokenConstants.TOKEN_TIMEOUT)
            };
        }

        private async Task<bool> IsUserLoggedIn(ResendTwoFactorAuthenticationTokenCommand request, IAuthenticationUser user, IToken? twoFactorPendingToken)
        {
            return user != null
                && twoFactorPendingToken != null
                && twoFactorPendingToken.UserId == request.UserId
                && (await tokenService.ValidateTokenAsync(user.Id, twoFactorPendingToken.Value, nameof(request.TwoFactorPendingToken)));
        }
    }
}
