using Bogus;
using Bogus.Extensions;
using Domain.CVS.Constants;
using Domain.CVS.Enums;
using TestData.Client;
using TestData.Organization;

namespace TestData.CoachingProgram
{
    public class CoachingProgramDataGenerator : ITestDataGenerator<Domain.CVS.Domain.CoachingProgram>
    {
        public Faker<Domain.CVS.Domain.CoachingProgram> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public CoachingProgramDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<Domain.CVS.Domain.CoachingProgram> GetFaker()
        {
            ITestDataGenerator<Domain.CVS.Domain.Client> testDataGeneratorClient = new ClientDataGenerator();

            return new AutoFaker<Domain.CVS.Domain.CoachingProgram>()
                .RuleFor(cp => cp.Id, 0)
                .RuleFor(cp => cp.Client, testDataGeneratorClient.Create())
                .RuleFor(cp => cp.Title, f => f.Commerce.ProductName())
                .RuleFor(cp => cp.OrderNumber, f => FillOptionalProperties ? f.Random.AlphaNumeric(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH) : null)
                .RuleFor(cp => cp.CoachingProgramType, f => f.PickRandom<CoachingProgramType>())
                .RuleFor(cp => cp.Organization, () =>
                {
                    ITestDataGenerator<Domain.CVS.Domain.Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();

                    return FillOptionalProperties ?
                        testDataGeneratorOrganization.Create()
                    : null;
                })
                .RuleFor(cp => cp.BeginDate, f =>
                {
                    var beginDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                    var endDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                    while (endDate <= beginDate)
                    {
                        endDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                    }
                    return beginDate;
                })
                .RuleFor(pc => pc.EndDate, (f, pc) =>
                {
                    var endDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                    while (endDate <= pc.BeginDate)
                    {
                        endDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
                    }
                    return endDate;
                })
                .RuleFor(cp => cp.BudgetAmmount, f => FillOptionalProperties ? f.Random.Decimal2(100, 10000) : null)
                .RuleFor(cp => cp.HourlyRate, f => f.Random.Decimal2(100, 250));
        }
    }
}
