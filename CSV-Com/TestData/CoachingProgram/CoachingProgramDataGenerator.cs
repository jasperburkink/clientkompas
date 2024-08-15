using Bogus;
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
                .RuleFor(cp => cp.Id, FakerConfiguration.Faker.IndexFaker++)
                .RuleFor(cp => cp.Client, testDataGeneratorClient.Create())
                .RuleFor(cp => cp.Title, f => f.Random.String2(CoachingProgramConstants.TITLE_MAXLENGTH))
                .RuleFor(cp => cp.OrderNumber, f => FillOptionalProperties ? f.Random.String2(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH) : null)
                .RuleFor(cp => cp.CoachingProgramType, f => f.PickRandom<CoachingProgramType>())
                .RuleFor(cp => cp.Organization, () =>
                {
                    ITestDataGenerator<Domain.CVS.Domain.Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();

                    return FillOptionalProperties ?
                        testDataGeneratorOrganization.Create()
                    : null;
                })
                .RuleFor(cp => cp.BeginDate, new DateOnlyBinder().CreateInstance<DateOnly>(null))
                .RuleFor(cp => cp.EndDate, new DateOnlyBinder().CreateInstance<DateOnly>(null))
                .RuleFor(cp => cp.BudgetAmmount, f => FillOptionalProperties ? f.Random.Decimal() : null)
                .RuleFor(cp => cp.HourlyRate, f => f.Random.Decimal());
        }
    }
}
