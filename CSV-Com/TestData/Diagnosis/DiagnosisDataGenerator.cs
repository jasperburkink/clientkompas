using Bogus;

namespace TestData.Diagnosis
{
    public class DiagnosisDataGenerator : ITestDataGenerator<Domain.CVS.Domain.Diagnosis>
    {
        public Faker<Domain.CVS.Domain.Diagnosis> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public DiagnosisDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<Domain.CVS.Domain.Diagnosis> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            return new AutoFaker<Domain.CVS.Domain.Diagnosis>()
                .RuleFor(d => d.Id, 0)
                .RuleFor(dl => dl.Name, f => f.PickRandom(Constants.DIAGNOSIS_OPTIONS));
        }
    }
}
