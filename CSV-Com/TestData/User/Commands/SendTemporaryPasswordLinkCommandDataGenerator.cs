using Application.Users.Commands.SendTemporaryPasswordLink;
using Bogus;

namespace TestData.User.Commands
{
    public class SendTemporaryPasswordLinkCommandDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<SendTemporaryPasswordLinkCommand>
    {
        public Faker<SendTemporaryPasswordLinkCommand> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<SendTemporaryPasswordLinkCommand> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<SendTemporaryPasswordLinkCommand>()
                .StrictMode(true)
                .RuleFor(c => c.UserId, Guid.NewGuid().ToString());
        }
    }
}
