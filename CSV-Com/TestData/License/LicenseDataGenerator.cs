using Bogus;
using Domain.CVS.Enums;

namespace TestData.License
{
    public class LicenseDataGenerator(bool fillOptionalProperties = true) : ITestDataGenerator<Domain.CVS.Domain.License>
    {
        public Faker<Domain.CVS.Domain.License> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; } = fillOptionalProperties;

        private Faker<Domain.CVS.Domain.License> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<Domain.CVS.Domain.License>()
                .RuleFor(l => l.Id, f => f.Random.Int())
                .RuleFor(l => l.Id, f => f.Random.Int(36))
                .RuleFor(l => l.CreatedAt, f => f.Date.Past())
                .RuleFor(l => l.ValidUntil, f => f.Date.Future())
                .RuleFor(l => l.Organization, f => new Domain.CVS.Domain.Organization { OrganizationName = f.Company.CompanyName() })
                .RuleFor(l => l.LicenseHolder, f => new Domain.CVS.Domain.User
                {
                    FirstName = f.Name.FirstName(),
                    LastName = f.Name.LastName(),
                    EmailAddress = f.Person.Email,
                    TelephoneNumber = f.Phone.PhoneNumber(),
                })
                .RuleFor(l => l.Status, f => f.PickRandom<LicenseStatus>());
        }

        public Domain.CVS.Domain.License Create()
        {
            return Faker.Generate();
        }

        public ICollection<Domain.CVS.Domain.License> Create(int count = 1)
        {
            return Faker.Generate(count);
        }
    }
}
