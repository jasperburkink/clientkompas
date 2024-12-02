using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<RefreshTokenCommandDto>
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandDto>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        private readonly IBearerTokenService _bearerTokenService;

        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IRefreshTokenService refreshTokenService, IBearerTokenService bearerTokenService, IIdentityService identityService)
        {
            _refreshTokenService = refreshTokenService;
            _bearerTokenService = bearerTokenService;
            _identityService = identityService;
        }

        public async Task<RefreshTokenCommandDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            var refreshTokenCurrent = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken) ?? throw new UnauthorizedAccessException("Token not found.");
            var validToken = await _refreshTokenService.ValidateRefreshTokenAsync(refreshTokenCurrent.UserId, request.RefreshToken);

            if (!validToken)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _identityService.GetUserAsync(refreshTokenCurrent.UserId) ?? throw new UnauthorizedAccessException("User not found.");
            var roles = await _identityService.GetRolesAsync(refreshTokenCurrent.UserId);

            await _refreshTokenService.RevokeRefreshTokenAsync(user.Id, request.RefreshToken);

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(user, roles);

            var refreshTokenNew = await _refreshTokenService.GenerateRefreshTokenAsync(user);

            return new RefreshTokenCommandDto
            {
                BearerToken = bearerToken,
                RefreshToken = refreshTokenNew,
                Success = true
            };
        }
    }
}
