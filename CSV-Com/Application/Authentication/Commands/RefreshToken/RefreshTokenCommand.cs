using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<RefreshTokenCommandDto>
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandDto>
    {
        private readonly ITokenService _tokenService;

        private readonly IBearerTokenService _bearerTokenService;

        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(ITokenService tokenService, IBearerTokenService bearerTokenService, IIdentityService identityService)
        {
            _tokenService = tokenService;
            _bearerTokenService = bearerTokenService;
            _identityService = identityService;
        }

        public async Task<RefreshTokenCommandDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            var refreshTokenCurrent = await _tokenService.GetTokenAsync(request.RefreshToken, "RefreshToken") ?? throw new UnauthorizedAccessException("Token not found.");
            var validToken = await _tokenService.ValidateTokenAsync(refreshTokenCurrent.UserId, request.RefreshToken, "RefreshToken");

            if (!validToken)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await _identityService.GetUserAsync(refreshTokenCurrent.UserId) ?? throw new UnauthorizedAccessException("User not found.");
            var roles = await _identityService.GetRolesAsync(refreshTokenCurrent.UserId);

            await _tokenService.RevokeTokenAsync(user.Id, request.RefreshToken, "RefreshToken");

            var bearerToken = await _bearerTokenService.GenerateBearerTokenAsync(user, roles);

            var refreshTokenNew = await _tokenService.GenerateTokenAsync(user, "RefreshToken");

            return new RefreshTokenCommandDto
            {
                BearerToken = bearerToken,
                RefreshToken = refreshTokenNew,
                Success = true
            };
        }
    }
}
