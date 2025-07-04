﻿using Bogus;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData.Diagnosis;
using TestData.DriversLicence;
using TestData.EmergencyPerson;
using TestData.MaritalStatus;
using TestData.Organization;
using TestData.WorkingContract;

namespace TestData.Client
{
    public class ClientDataGenerator : ITestDataGenerator<Domain.CVS.Domain.Client>
    {
        public Faker<Domain.CVS.Domain.Client> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public ClientDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<Domain.CVS.Domain.Client> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);
            var random = new Random();

            ITestDataGenerator<Domain.CVS.Domain.MaritalStatus> testDataGeneratorMaritalStatus = new MaritalStatusDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.DriversLicence> testDataGeneratorDriverLicence = new DriversLicenceDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.Diagnosis> testDataGeneratorDiagnosis = new DiagnosisDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.EmergencyPerson> testDataGeneratorEmergencyPerson = new EmergencyPersonDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.WorkingContract> testDataGeneratorWorkingContract = new WorkingContractDataGenerator();

            var autofaker = new AutoFaker<Domain.CVS.Domain.Client>()
                .RuleFor(c => c.Id, 0)
                .RuleFor(c => c.FirstName, faker.Person.FirstName)
                .RuleFor(c => c.Initials, faker.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
                .RuleFor(c => c.PrefixLastName, f =>
                {
                    var usePrefixLastName = random.Next(2) == 1;
                    return usePrefixLastName ? faker.PickRandom(Constants.PREFIX_LASTNAME_OPTIONS) : null;
                })
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
                .RuleFor(c => c.DeactivationDateTime, f => null);

            if (FillOptionalProperties)
            {
                autofaker.RuleFor(c => c.MaritalStatus, f => testDataGeneratorMaritalStatus.Create())
                .RuleFor(c => c.BenefitForms, f => f.Make(3, () => new BenefitForm { Id = 0, Name = f.PickRandom(Constants.BENEFITFORM_OPTIONS) }))
                .RuleFor(c => c.DriversLicences, f => f.Make(3, () => testDataGeneratorDriverLicence.Create()))
                .RuleFor(c => c.Diagnoses, f => f.Make(3, () => testDataGeneratorDiagnosis.Create()))
                .RuleFor(c => c.EmergencyPeople, f => f.Make(3, () => testDataGeneratorEmergencyPerson.Create()))
                .RuleFor(c => c.WorkingContracts, f => f.Make(5, () => testDataGeneratorWorkingContract.Create()))
                .RuleFor(c => c.Remarks, faker.Lorem.Sentence());
            }
            else
            {
                // Geen optionele eigenschappen, dus zet deze op null of minimale waarden
                autofaker.RuleFor(c => c.MaritalStatus, f => null)
                .RuleFor(c => c.BenefitForms, f => null)
                .RuleFor(c => c.DriversLicences, f => new())
                .RuleFor(c => c.Diagnoses, f => new())
                .RuleFor(c => c.EmergencyPeople, f => new())
                .RuleFor(c => c.WorkingContracts, f => new())
                .RuleFor(c => c.Remarks, (string)null);
            }

            return autofaker;
        }
    }
}
