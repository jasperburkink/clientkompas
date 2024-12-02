using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest<LogoutCommandDto>
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutCommandHandler(IIdentityService identityService, IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LogoutCommandDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            await _identityService.LogoutAsync();

            var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);

            if (refreshToken == null)
            {
                return new LogoutCommandDto
                {
                    Success = true
                };
            }

            var userRefreshTokens = await _refreshTokenService.GetValidRefreshTokensByUserAsync(refreshToken.UserId);

            foreach (var userRefreshToken in userRefreshTokens)
            {
                await _refreshTokenService.RevokeRefreshTokenAsync(userRefreshToken.UserId, userRefreshToken.Value);
            }

            return new LogoutCommandDto
            {
                Success = true
            };
        }
    }
}
