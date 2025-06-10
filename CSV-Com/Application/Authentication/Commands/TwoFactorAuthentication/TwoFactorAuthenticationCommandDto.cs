namespace Application.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandDto
    {
        public required bool Success { get; set; }

        public string BearerToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
    }
}
