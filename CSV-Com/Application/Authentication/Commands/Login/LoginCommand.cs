using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;

namespace Application.Authentication.Commands.Login
{
    public record LoginCommand : IRequest<LoginCommandDto>
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IResourceMessageProvider _resourceMessageProvider;
        private readonly IEmailService _emailService;

        public LoginCommandHandler(IIdentityService identityService, IBearerTokenService bearerTokenService, IRefreshTokenService refreshTokenService, IResourceMessageProvider resourceMessageProvider, IEmailService emailService)
        {
            _identityService = identityService;
            _bearerTokenService = bearerTokenService;
            _refreshTokenService = refreshTokenService;
            _resourceMessageProvider = resourceMessageProvider;
            _emailService = emailService;
        }

        public async Task<LoginCommandDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loggedInUser = await _identityService.LoginAsync(request.UserName!, request.Password!);

            if (IsInvalidLogin(loggedInUser))
            {
                throw new InvalidLoginException(_resourceMessageProvider.GetMessage(typeof(LoginCommandHandler), "InvalidLogin"));
            }

            if (loggedInUser.User.TwoFactorEnabled)
            {
                var twoFactorAuthenticationToken = await _identityService.Get2FATokenAsync(loggedInUser.User.Id);

                // Send the token via email
                await _emailService.SendEmailAsync(loggedInUser.User.Email, "Two-factor authentication token", twoFactorAuthenticationToken);

                return new LoginCommandDto
                {
                    Success = true,
                    UserId = loggedInUser.User.Id,
                    TwoFactorAuthenticationToken = twoFactorAuthenticationToken
                };
            }
            else
            {
                var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(loggedInUser.User, loggedInUser.Roles); // UserInfo & roles are processed inside the bearertoken claims
                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(loggedInUser.User);

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
