using Bogus;

namespace TestData.User
{
    public class UserDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<Domain.CVS.Domain.User>
    {
        public Faker<Domain.CVS.Domain.User> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<Domain.CVS.Domain.User> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);
            var random = new Random();

            return new AutoFaker<Domain.CVS.Domain.User>()
                .StrictMode(false)
                .RuleFor(u => u.Id, 0)
                .RuleFor(u => u.EmailAddress, faker.Person.Email)
                .RuleFor(u => u.FirstName, faker.Person.FirstName)
                .RuleFor(u => u.PrefixLastName, f =>
                {
                    var usePrefixLastName = random.Next(2) == 1;
                    return usePrefixLastName ? faker.PickRandom(Constants.PREFIX_LASTNAME_OPTIONS) : null;
                })
                .RuleFor(u => u.LastName, faker.Person.LastName)
                .RuleFor(u => u.TelephoneNumber, faker.Phone.PhoneNumber())
                .RuleFor(u => u.DeactivationDateTime, f => { return null; })
                .RuleFor(u => u.CreatedByUser, f => { return null; })
                .RuleFor(u => u.CreatedByUserId, f => { return null; });
        }
    }
}
