
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

    public class ResendTwoFactorAuthenticationTokenCommandHandler : IRequestHandler<ResendTwoFactorAuthenticationTokenCommand, ResendTwoFactorAuthenticationTokenCommandDto>
    {
        private const string RESOURCE_KEY_NOEMAILADDRESS = "NoEmailAddress";
        private const string RESOURCE_KEY_USERNOTLOGGEDIN = "UserNotLoggedIn";
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;
        private readonly IEmailService _emailService;

        public ResendTwoFactorAuthenticationTokenCommandHandler(IIdentityService identityService, ITokenService tokenService, IResourceMessageProvider resourceMessageProvider, IEmailService emailService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _resourceMessageProvider = resourceMessageProvider;
            _emailService = emailService;
        }

        public async Task<ResendTwoFactorAuthenticationTokenCommandDto> Handle(ResendTwoFactorAuthenticationTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserAsync(request.UserId);
            var currentTwoFactorPendingToken = await _tokenService.GetTokenAsync(request.TwoFactorPendingToken, nameof(request.TwoFactorPendingToken));

            if (!await IsUserLoggedIn(request, user, currentTwoFactorPendingToken))
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(TwoFactorAuthenticationCommandHandler), RESOURCE_KEY_USERNOTLOGGEDIN));
            }

            // Token value that user needs to enter
            var twoFactorAuthenticationTokenValue = await _identityService.Get2FATokenAsync(user.Id);

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new NotFoundException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), RESOURCE_KEY_NOEMAILADDRESS));
            }

            // Security token for checking loginstatus user
            var twoFactorPendingTokenValue = await _tokenService.GenerateTokenAsync(user, nameof(request.TwoFactorPendingToken));

            // Send the token via email
            await _emailService.SendEmailAsync(user.Email, "Two-factor authentication token", twoFactorAuthenticationTokenValue);

            return new ResendTwoFactorAuthenticationTokenCommandDto
            {
                Success = true,
                UserId = user.Id,
                TwoFactorPendingToken = twoFactorPendingTokenValue,
                ExpiresAt = DateTime.UtcNow.Add(TwoFactorPendingTokenConstants.TOKEN_TIMEOUT)
            };
        }

        private async Task<bool> IsUserLoggedIn(ResendTwoFactorAuthenticationTokenCommand request, AuthenticationUser user, IToken? twoFactorPendingToken)
        {
            return user != null
                && twoFactorPendingToken != null
                && twoFactorPendingToken.UserId == request.UserId
                && (await _tokenService.ValidateTokenAsync(user.Id, twoFactorPendingToken.Value, nameof(request.TwoFactorPendingToken)));
        }
    }
}
