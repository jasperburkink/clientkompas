namespace EmailModule
{
    internal static class EmailConfig
    {
        internal const string SmtpServer = "mail.mijndomein.nl";

        internal const int Port = 587;
        //public const int Port = 465;

        internal const string Username = "ontwikkelaar@clientkompas.nl";

        internal const string Password = "iaOQ6m6hsm4cK5yAEi1X";

        internal const bool UseSsl = true;

        internal const bool RequiresAuthentication = true;
    }
}
