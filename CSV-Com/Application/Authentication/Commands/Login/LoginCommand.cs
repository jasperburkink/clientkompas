using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;

namespace Application.Authentication.Commands.Login
{
    public record LoginCommand : IRequest<LoginCommandDto>
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandDto>
    {
        private const string RESOURCE_KEY_INVALIDLOGIN = "InvalidLogin";
        private const string RESOURCE_KEY_NOEMAILADDRESS = "NoEmailAddress";
        private readonly IIdentityService _identityService;
        private readonly IBearerTokenService _bearerTokenService;
        private readonly ITokenService _tokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;
        private readonly IEmailService _emailService;

        public LoginCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, ITokenService tokenService, IResourceMessageProvider resourceMessageProvider, IEmailService emailService)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
            _tokenService = tokenService;
            _resourceMessageProvider = resourceMessageProvider;
            _emailService = emailService;
        }

        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _identityService.LoginAsync(request.UserName!, request.Password!);

            if (IsInvalidLogin(loggedInUser))
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), RESOURCE_KEY_INVALIDLOGIN));
            }

            if (loggedInUser.User.TwoFactorEnabled)
            {
                // Token value that user needs to enter
                var twoFactorAuthenticationTokenValue = await _identityService.Get2FATokenAsync(loggedInUser.User.Id);

                if (string.IsNullOrEmpty(loggedInUser.User.Email))
                {
                    throw new NotFoundException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), RESOURCE_KEY_NOEMAILADDRESS));
                }

                // Security token for checking loginstatus user
                var twoFactorPendingTokenValue = await _tokenService.GenerateTokenAsync(loggedInUser.User, nameof(LoginCommandDto.TwoFactorPendingToken));

                // Send the token via email TODO: user emailmodule
                await _emailService.SendEmailAsync(loggedInUser.User.Email, "Two-factor authentication token", twoFactorAuthenticationTokenValue);

                return new LoginCommandDto
                {
                    Success = true,
                    UserId = loggedInUser.User.Id,
                    TwoFactorPendingToken = twoFactorPendingTokenValue,
                    ExpiresAt = DateTime.UtcNow.Add(TwoFactorPendingTokenConstants.TOKEN_TIMEOUT)
                };
            }
            else
            {
                var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(loggedInUser.User, loggedInUser.Roles); // UserInfo & roles are processed inside the bearertoken claims
                var refreshToken = await _tokenService.GenerateTokenAsync(loggedInUser.User, nameof(LoginCommandDto.RefreshToken));

                return new LoginCommandDto
                {
                    Success = true,
                    BearerToken = bearerToken,
                    RefreshToken = refreshToken
                };
            }
        }

        private static bool IsInvalidLogin(LoggedInResult loggedInUser)
        {
            return !loggedInUser.Succeeded || loggedInUser.User == null || loggedInUser.Roles == null;
        }
    }
}
