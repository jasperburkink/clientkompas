using Bogus;
using Domain.CVS.Enums;
using TestData.Client;
using TestData.Organization;

namespace TestData.WorkingContract
{
    public class WorkingContractDataGenerator : ITestDataGenerator<Domain.CVS.Domain.WorkingContract>
    {
        public Faker<Domain.CVS.Domain.WorkingContract> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public WorkingContractDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        public Faker<Domain.CVS.Domain.WorkingContract> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            ITestDataGenerator<Domain.CVS.Domain.Client> testDataGeneratorClient = new ClientDataGenerator();
            ITestDataGenerator<Domain.CVS.Domain.Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();

            return new AutoFaker<Domain.CVS.Domain.WorkingContract>()
            .RuleFor(wc => wc.Id, f => 0)
            .RuleFor(wc => wc.Organization, f => testDataGeneratorOrganization.Create())
            .RuleFor(wc => wc.Function, f => faker.Name.JobTitle())
            .RuleFor(wc => wc.ContractType, f => f.PickRandom<ContractType>())
            .RuleFor(wc => wc.FromDate, f =>
            {
                var fromDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                var toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                while (toDate <= fromDate)
                {
                    toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                }
                return fromDate;
            })
            .RuleFor(wc => wc.ToDate, (f, wc) =>
            {
                var toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                while (toDate <= wc.FromDate)
                {
                    toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                }
                return toDate;
            });
        }
    }
}
