using Application.Common.Interfaces.Authentication;

namespace Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<RefreshTokenCommandDto>
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler(ITokenService tokenService, IBearerTokenService bearerTokenService, IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandDto>
    {
        public async Task<RefreshTokenCommandDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.RefreshToken);

            var refreshTokenCurrent = await tokenService.GetTokenAsync(request.RefreshToken, "RefreshToken") ?? throw new UnauthorizedAccessException("Token not found.");
            var validToken = await tokenService.ValidateTokenAsync(refreshTokenCurrent.UserId, request.RefreshToken, "RefreshToken");

            if (!validToken)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            var user = await identityService.GetUserAsync(refreshTokenCurrent.UserId) ?? throw new UnauthorizedAccessException("User not found.");
            var roles = await identityService.GetRolesAsync(refreshTokenCurrent.UserId);

            await tokenService.RevokeTokenAsync(user.Id, request.RefreshToken, "RefreshToken");

            var bearerToken = await bearerTokenService.GenerateBearerTokenAsync(user, roles);

            var refreshTokenNew = await tokenService.GenerateTokenAsync(user, "RefreshToken");

            return new RefreshTokenCommandDto
            {
                BearerToken = bearerToken,
                RefreshToken = refreshTokenNew,
                Success = true
            };
        }
    }
}
