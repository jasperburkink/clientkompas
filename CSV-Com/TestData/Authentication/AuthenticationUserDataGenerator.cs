using System.Text;
using Bogus;
using Infrastructure.Identity;

namespace TestData.Authentication
{
    public class AuthenticationUserDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<AuthenticationUser>
    {
        public Faker<AuthenticationUser> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<AuthenticationUser> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);
            var random = new Random();

            var autofaker = new AutoFaker<AuthenticationUser>()
                .RuleFor(c => c.CVSUserId, faker.Random.Int(min: 0))
                .RuleFor(c => c.Email, faker.Person.Email)
                .RuleFor(c => c.UserName, faker.Person.Email)
                .RuleFor(c => c.Salt, Encoding.UTF8.GetBytes("Salt"));

            return autofaker;
        }
    }
}
