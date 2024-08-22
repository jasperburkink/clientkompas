using Bogus;

namespace TestData.MaritalStatus
{
    public class MaritalStatusDataGenerator : ITestDataGenerator<Domain.CVS.Domain.MaritalStatus>
    {
        public Faker<Domain.CVS.Domain.MaritalStatus> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public MaritalStatusDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<Domain.CVS.Domain.MaritalStatus> GetFaker()
        {
            return new AutoFaker<Domain.CVS.Domain.MaritalStatus>()
                .RuleFor(mt => mt.Name, f => f.PickRandom(Constants.MARITALSTATUSES_OPTIONS));
        }
    }
}
