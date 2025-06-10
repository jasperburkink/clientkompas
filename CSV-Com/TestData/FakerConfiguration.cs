using Bogus;

namespace TestData
{
    public static class FakerConfiguration
    {
        public static Faker Faker => new(Localization) { IndexFaker = 1 };
        public const string Localization = "nl";

        static FakerConfiguration()
        {
            AutoFaker.Configure(builder =>
            {
                builder.WithBinder(new DateOnlyBinder());
            });
        }
    }
}
