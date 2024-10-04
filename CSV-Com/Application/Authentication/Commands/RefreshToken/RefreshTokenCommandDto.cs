namespace Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandDto
    {
        public required bool Success { get; set; }

        public string? BearerToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
