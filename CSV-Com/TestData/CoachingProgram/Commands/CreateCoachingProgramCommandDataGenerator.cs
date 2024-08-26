using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Bogus;
using Bogus.Extensions;
using Domain.CVS.Constants;
using Domain.CVS.Enums;

namespace TestData.CoachingProgram.Commands
{
    public class CreateCoachingProgramCommandDataGenerator : ITestDataGenerator<CreateCoachingProgramCommand>
    {
        public Faker<CreateCoachingProgramCommand> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public CreateCoachingProgramCommandDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<CreateCoachingProgramCommand> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            var fromDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
            var toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);

            while (toDate <= fromDate)
            {
                toDate = new DateOnlyBinder().CreateInstance<DateOnly>(null);
            }

            return new AutoFaker<CreateCoachingProgramCommand>()
                .StrictMode(true)
                .RuleFor(c => c.ClientId, FakerConfiguration.Faker.IndexFaker++)
                .RuleFor(c => c.Title, faker.Random.String2(CoachingProgramConstants.TITLE_MAXLENGTH))
                .RuleFor(c => c.OrderNumber, FillOptionalProperties ? faker.Random.String2(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH) : null)
                .RuleFor(c => c.OrganizationId, FakerConfiguration.Faker.IndexFaker++)
                .RuleFor(c => c.CoachingProgramType, faker.PickRandom<CoachingProgramType>())
                .RuleFor(c => c.BeginDate, fromDate)
                .RuleFor(c => c.EndDate, toDate)
                .RuleFor(c => c.BudgetAmmount, FillOptionalProperties ? faker.Random.Decimal2(min: 1000m, max: 10000m) : null)
                .RuleFor(c => c.HourlyRate, faker.Random.Decimal2(min: 10m, max: 100m));
        }
    }
}
