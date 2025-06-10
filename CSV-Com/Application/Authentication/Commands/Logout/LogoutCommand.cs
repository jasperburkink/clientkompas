using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest<LogoutCommandDto>
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class LogoutCommandHandler(IIdentityService identityService, ITokenService tokenService) : IRequestHandler<LogoutCommand, LogoutCommandDto>
    {
        public async Task<LogoutCommandDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            await identityService.LogoutAsync();

            var refreshToken = await tokenService.GetTokenAsync(request.RefreshToken, "RefreshToken");

            if (refreshToken == null)
            {
                return new LogoutCommandDto
                {
                    Success = true
                };
            }

            var userRefreshTokens = await tokenService.GetValidTokensByUserAsync(refreshToken.UserId, "RefreshToken");

            foreach (var userRefreshToken in userRefreshTokens)
            {
                await tokenService.RevokeTokenAsync(userRefreshToken.UserId, userRefreshToken.Value, "RefreshToken");
            }

            return new LogoutCommandDto
            {
                Success = true
            };
        }
    }
}
