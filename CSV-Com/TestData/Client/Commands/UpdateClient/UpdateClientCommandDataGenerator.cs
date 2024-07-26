using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Commands.UpdateClient;
using Application.Clients.Dtos;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Bogus;
using Domain.CVS.Enums;

namespace TestData.Client.Commands.UpdateClient
{
    public class UpdateClientCommandDataGenerator : ITestDataGenerator<UpdateClientCommand>
    {
        public Faker<UpdateClientCommand> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public UpdateClientCommandDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<UpdateClientCommand> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<UpdateClientCommand>()
                .StrictMode(true)
                .RuleFor(c => c.Id, f => f.Random.Int())
                .RuleFor(c => c.FirstName, faker.Person.FirstName)
                .RuleFor(c => c.Initials, faker.Random.String2(1, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
                .RuleFor(c => c.PrefixLastName, faker.Random.String2(3, "abcdefghijklmnopqrstuvwxyz"))
                .RuleFor(c => c.LastName, faker.Person.LastName)
                .RuleFor(c => c.Gender, faker.PickRandom<Gender>())
                .RuleFor(c => c.StreetName, faker.Address.StreetName())
                .RuleFor(c => c.HouseNumber, faker.Random.Int(1, 1000))
                .RuleFor(c => c.HouseNumberAddition, faker.Random.String2(1, "A"))
                .RuleFor(c => c.PostalCode, new PostalCodeBinder().CreateInstance<string>(null))
                .RuleFor(c => c.Residence, faker.Address.City())
                .RuleFor(c => c.TelephoneNumber, faker.Phone.PhoneNumber())
                .RuleFor(c => c.DateOfBirth, f => new DateOnlyBinder().CreateInstance<DateOnly>(null))
                .RuleFor(c => c.EmailAddress, faker.Person.Email)
                .RuleFor(c => c.MaritalStatus, f => new MaritalStatusDto { Id = f.Random.Int(1, 10), Name = f.Random.String2(10) })
                .RuleFor(c => c.BenefitForms, f => f.Make(3, () => new BenefitFormDto { Id = f.Random.Int(1, 10), Name = f.Random.String2(10) }))
                .RuleFor(c => c.DriversLicences, f => f.Make(3, () => new DriversLicenceDto { Id = f.Random.Int(1, 10), Category = f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") }))
                .RuleFor(c => c.Diagnoses, f => f.Make(3, () => new DiagnosisDto() { Id = f.Random.Int(1, 10), Name = f.Random.String2(10) }))
                .RuleFor(c => c.EmergencyPeople, f => f.Make(3, () => new EmergencyPersonDto { Name = faker.Person.FullName, TelephoneNumber = faker.Phone.PhoneNumber() }))
                .RuleFor(c => c.WorkingContracts, f => f.Make(5, () => new ClientWorkingContractDto
                {
                    Id = 0,
                    FromDate = new DateOnlyBinder().CreateInstance<DateOnly>(null),
                    ToDate = new DateOnlyBinder().CreateInstance<DateOnly>(null),
                    ContractType = f.PickRandom<ContractType>(),
                    Function = faker.Name.JobTitle(),
                    OrganizationId = f.Random.Int(1, 10)
                }))
                .RuleFor(c => c.Remarks, faker.Lorem.Sentence());
        }
    }
}
