using Bogus;

namespace TestData.EmergencyPerson
{
    public class EmergencyPersonDataGenerator : ITestDataGenerator<Domain.CVS.Domain.EmergencyPerson>
    {
        public Faker<Domain.CVS.Domain.EmergencyPerson> Faker { get => GetFaker(); }

        public Faker<Domain.CVS.Domain.EmergencyPerson> GetFaker()
        {
            return new AutoFaker<Domain.CVS.Domain.EmergencyPerson>()
                .RuleFor(ep => ep.Name, FakerConfiguration.Faker.Person.FullName)
                .RuleFor(ep => ep.TelephoneNumber, FakerConfiguration.Faker.Phone.PhoneNumber());
        }
    }
}
