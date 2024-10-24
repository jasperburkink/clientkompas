namespace Domain.Authentication.Constants
{
    public class BearerTokenConstants
    {
        public const string ISSUER = "https://CliëntKompas.nl";
        public const string AUDIENCE = "https://CliëntKompas.nl";
        public static readonly TimeSpan TOKEN_TIMEOUT = TimeSpan.FromMinutes(30);
    }
}
