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
                .RuleFor(mt => mt.Name, f => f.Random.String2(10)); // TODO: Take value 10 from contants
        }
    }
}
