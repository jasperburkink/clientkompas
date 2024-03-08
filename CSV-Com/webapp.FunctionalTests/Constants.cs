namespace WebApp.FunctionalTests
{
    internal static class Constants
    {
#if DEBUG
        public const string Url = "http://localhost:3000/";
#else
        public static readonly string Url = Environment.GetEnvironmentVariable("WebAppUrl");
#endif
    }
}
