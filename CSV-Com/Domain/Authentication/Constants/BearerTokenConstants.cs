namespace Domain.Authentication.Constants
{
    public class BearerTokenConstants
    {
        public const string ISSUER = "https://ClientKompas.nl";
        public const string AUDIENCE = "https://ClientKompas.nl";
        public static readonly TimeSpan TOKEN_TIMEOUT = TimeSpan.FromHours(2);
    }
}
