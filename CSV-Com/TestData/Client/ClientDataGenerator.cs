using Bogus;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData.Diagnosis;
using TestData.DriversLicence;
using TestData.EmergencyPerson;
using TestData.MaritalStatus;
using TestData.Organization;

namespace TestData.Client
{
    public class ClientDataGenerator : ITestDataGenerator<Domain.CVS.Domain.Client>
    {
        public Faker<Domain.CVS.Domain.Client> Faker { get => GetFaker(); }

        private Faker<Domain.CVS.Domain.Client> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            ITestDataGenerator<Domain.CVS.Domain.MaritalStatus> testDataGeneratorMaritalStatus = new MaritalStatusDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.DriversLicence> testDataGeneratorDriverLicence = new DriversLicenceDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.Diagnosis> testDataGeneratorDiagnosis = new DiagnosisDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.EmergencyPerson> testDataGeneratorEmergencyPerson = new EmergencyPersonDataGenerator();

            return new AutoFaker<Domain.CVS.Domain.Client>()
                .RuleFor(c => c.Id, 0)
                .RuleFor(c => c.FirstName, faker.Person.FirstName)
                .RuleFor(c => c.Initials, faker.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
                .RuleFor(c => c.PrefixLastName, faker.Random.String2(3, "abcdefghijklmnopqrstuvwxyz"))
                .RuleFor(c => c.LastName, faker.Person.LastName)
                .RuleFor(c => c.Gender, faker.PickRandom<Gender>())
                .RuleFor(c => c.Address, Domain.CVS.ValueObjects.Address.From(
                    faker.Address.StreetName(),
                    faker.Random.Int(1, 1000),
                    faker.Random.String2(1, "A"),
                    new PostalCodeBinder().CreateInstance<string>(null),
                    faker.Address.City()))
                .RuleFor(c => c.TelephoneNumber, faker.Phone.PhoneNumber())
                .RuleFor(c => c.DateOfBirth, new DateOnlyBinder().CreateInstance<DateOnly>(null))
                .RuleFor(c => c.EmailAddress, faker.Person.Email)
                .RuleFor(c => c.DeactivationDateTime, f => null)
                .RuleFor(c => c.MaritalStatus, f => testDataGeneratorMaritalStatus.Create())
                .RuleFor(c => c.BenefitForms, f => f.Make(3, () => new BenefitForm { Id = 0, Name = f.Random.String2(10) }))
                .RuleFor(c => c.DriversLicences, f => f.Make(3, () => testDataGeneratorDriverLicence.Create()))
                .RuleFor(c => c.Diagnoses, f => f.Make(3, () => testDataGeneratorDiagnosis.Create()))
                .RuleFor(c => c.EmergencyPeople, f => f.Make(3, () => testDataGeneratorEmergencyPerson.Create()))
                .RuleFor(c => c.WorkingContracts, f => f.Make(5, () => new WorkingContract
                {
                    Id = 0,
                    FromDate = new DateOnlyBinder().CreateInstance<DateOnly>(null),
                    ToDate = new DateOnlyBinder().CreateInstance<DateOnly>(null),
                    ContractType = f.PickRandom<ContractType>(),
                    Function = faker.Name.JobTitle(),
                    Organization = testDataGeneratorOrganization.Create()
                }))
                .RuleFor(c => c.Remarks, faker.Lorem.Sentence());
        }
    }
}
