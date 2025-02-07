using System.Text;
using Bogus;
using Domain.Authentication.Domain;

namespace TestData.Authentication
{
    public class AuthenticationUserDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<IAuthenticationUser>
    {
        public Faker<IAuthenticationUser> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<IAuthenticationUser> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);
            var random = new Random();

            var autofaker = new AutoFaker<IAuthenticationUser>()
                .RuleFor(c => c.CVSUserId, faker.Random.Int(min: 0))
                .RuleFor(c => c.Email, faker.Person.Email)
                .RuleFor(c => c.UserName, faker.Person.Email)
                .RuleFor(c => c.Salt, Encoding.UTF8.GetBytes("Salt"));

            return autofaker;
        }
    }
}
