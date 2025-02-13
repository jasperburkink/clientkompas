using Application.Users.Commands.CreateUser;
using Bogus;
using Domain.Authentication.Constants;

namespace TestData.User.Commands
{
    public class CreateUserCommandDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<CreateUserCommand>
    {
        public Faker<CreateUserCommand> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<CreateUserCommand> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<CreateUserCommand>()
                .StrictMode(true)
                .RuleFor(c => c.FirstName, faker.Person.FirstName)
                .RuleFor(c => c.PrefixLastName, faker.Random.String2(3, "abcdefghijklmnopqrstuvwxyz"))
                .RuleFor(c => c.LastName, faker.Person.LastName)
                .RuleFor(c => c.EmailAddress, faker.Person.Email)
                .RuleFor(c => c.TelephoneNumber, faker.Phone.PhoneNumber())
                .RuleFor(c => c.RoleName, faker.PickRandom(new List<string>() { Roles.SystemOwner, Roles.Administrator, Roles.Licensee, Roles.Coach }));
        }
    }
}
