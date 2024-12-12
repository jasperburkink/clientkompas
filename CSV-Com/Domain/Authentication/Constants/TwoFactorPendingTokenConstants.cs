namespace Domain.Authentication.Constants
{
    public class TwoFactorPendingTokenConstants
    {
        public static readonly string NAME = "Client Kompas TwoFactorPendingToken";
        public static readonly TimeSpan TOKEN_TIMEOUT = TimeSpan.FromMinutes(15);
    }
}
