using Bogus;

namespace TestData.EmergencyPerson
{
    public class EmergencyPersonDataGenerator : ITestDataGenerator<Domain.CVS.Domain.EmergencyPerson>
    {
        public bool FillOptionalProperties { get; set; }

        public Faker<Domain.CVS.Domain.EmergencyPerson> Faker { get => GetFaker(); }

        public Faker<Domain.CVS.Domain.EmergencyPerson> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<Domain.CVS.Domain.EmergencyPerson>()
                .RuleFor(ep => ep.Name, faker.Person.FullName)
                .RuleFor(ep => ep.TelephoneNumber, FakerConfiguration.Faker.Phone.PhoneNumber());
        }
    }
}
