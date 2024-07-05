using Bogus;

namespace TestData.Diagnosis
{
    public class DiagnosisDataGenerator : ITestDataGenerator<Domain.CVS.Domain.Diagnosis>
    {
        public Faker<Domain.CVS.Domain.Diagnosis> Faker { get => GetFaker(); }

        private Faker<Domain.CVS.Domain.Diagnosis> GetFaker()
        {
            return new AutoFaker<Domain.CVS.Domain.Diagnosis>()
                .RuleFor(d => d.Id, 0)
                .RuleFor(dl => dl.Name, f => f.Random.String2(10));
        }
    }
}
