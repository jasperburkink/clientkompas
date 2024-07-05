using Bogus;

namespace TestData.DriversLicence
{
    public class DriversLicenceDataGenerator : ITestDataGenerator<Domain.CVS.Domain.DriversLicence>
    {
        public Faker<Domain.CVS.Domain.DriversLicence> Faker { get => GetFaker(); }

        private Faker<Domain.CVS.Domain.DriversLicence> GetFaker()
        {
            return new AutoFaker<Domain.CVS.Domain.DriversLicence>()
                .RuleFor(dl => dl.Id, 0)
                .RuleFor(dl => dl.Category, f => f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
                .RuleFor(dl => dl.Description, f => f.Random.String2(10));
        }
    }
}
