using Bogus;

namespace TestData.Organization
{
    public class OrganizationDataGenerator : ITestDataGenerator<Domain.CVS.Domain.Organization>
    {
        public Faker<Domain.CVS.Domain.Organization> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public OrganizationDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        public Faker<Domain.CVS.Domain.Organization> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<Domain.CVS.Domain.Organization>()
                .RuleFor(o => o.OrganizationName, FakerConfiguration.Faker.Company.CompanyName())
                .RuleFor(o => o.VisitAddress, Domain.CVS.ValueObjects.Address.From(
                    FakerConfiguration.Faker.Address.StreetName(),
                    FakerConfiguration.Faker.Random.Int(1, 1000),
                    FakerConfiguration.Faker.Random.String2(1, "A"),
                    new PostalCodeBinder().CreateInstance<string>(null),
                    FakerConfiguration.Faker.Address.City()))
                .RuleFor(o => o.InvoiceAddress, Domain.CVS.ValueObjects.Address.From(
                    FakerConfiguration.Faker.Address.StreetName(),
                    FakerConfiguration.Faker.Random.Int(1, 1000),
                    FakerConfiguration.Faker.Random.String2(1, "A"),
                    new PostalCodeBinder().CreateInstance<string>(null),
                    FakerConfiguration.Faker.Address.City()))
                .RuleFor(o => o.PostAddress, Domain.CVS.ValueObjects.Address.From(
                    FakerConfiguration.Faker.Address.StreetName(),
                    FakerConfiguration.Faker.Random.Int(1, 1000),
                    FakerConfiguration.Faker.Random.String2(1, "A"),
                    new PostalCodeBinder().CreateInstance<string>(null),
                    FakerConfiguration.Faker.Address.City()))
                .RuleFor(o => o.ContactPersonName, FakerConfiguration.Faker.Person.FullName)
                .RuleFor(o => o.ContactPersonFunction, FakerConfiguration.Faker.Name.JobTitle)
                .RuleFor(o => o.ContactPersonTelephoneNumber, FakerConfiguration.Faker.Phone.PhoneNumber())
                .RuleFor(o => o.ContactPersonMobilephoneNumber, FakerConfiguration.Faker.Phone.PhoneNumber())
                .RuleFor(o => o.ContactPersonEmailAddress, FakerConfiguration.Faker.Person.Email)
                .RuleFor(o => o.PhoneNumber, FakerConfiguration.Faker.Phone.PhoneNumber())
                .RuleFor(o => o.Website, FakerConfiguration.Faker.Internet.Url)
                .RuleFor(o => o.EmailAddress, FakerConfiguration.Faker.Person.Email)
                .RuleFor(o => o.KVKNumber, f => f.Random.Replace("########"))
                .RuleFor(o => o.BTWNumber, f => f.Random.Replace("#########B##"))
                .RuleFor(o => o.IBANNumber, f => f.Finance.Iban(false))
                .RuleFor(o => o.BIC, FakerConfiguration.Faker.Finance.Bic());
        }
    }
}
