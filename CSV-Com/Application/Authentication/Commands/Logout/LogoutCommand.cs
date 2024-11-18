using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest<LogoutCommandDto>
    {
        public string? RefreshToken { get; set; }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<LogoutCommandDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            await _identityService.LogoutAsync();

            var refreshToken = await _tokenService.GetTokenAsync(request.RefreshToken, "RefreshToken");

            if (refreshToken == null)
            {
                return new LogoutCommandDto
                {
                    Success = true
                };
            }

            var userRefreshTokens = await _tokenService.GetValidTokensByUserAsync(refreshToken.UserId, "RefreshToken");

            foreach (var userRefreshToken in userRefreshTokens)
            {
                await _tokenService.RevokeTokenAsync(userRefreshToken.UserId, userRefreshToken.Value, "RefreshToken");
            }

            return new LogoutCommandDto
            {
                Success = true
            };
        }
    }
}
