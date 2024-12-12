namespace Application.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandDto
    {
        public required bool Success { get; set; }

        public string UserId { get; set; } = null!;

        public string TwoFactorPendingToken { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }
    }
}
