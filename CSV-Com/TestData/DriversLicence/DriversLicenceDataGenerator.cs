using Bogus;

namespace TestData.DriversLicence
{
    public class DriversLicenceDataGenerator : ITestDataGenerator<Domain.CVS.Domain.DriversLicence>
    {
        public Faker<Domain.CVS.Domain.DriversLicence> Faker { get => GetFaker(); }

        public bool FillOptionalProperties { get; set; }

        public DriversLicenceDataGenerator(bool fillOptionalProperties = true)
        {
            FillOptionalProperties = fillOptionalProperties;
        }

        private Faker<Domain.CVS.Domain.DriversLicence> GetFaker()
        {
            var faker = new Faker(FakerConfiguration.Localization);

            var random = new Random();
            var index = random.Next(Constants.DRIVERSLICENCE_CATEGORIES.Count());

            return new AutoFaker<Domain.CVS.Domain.DriversLicence>()
                .RuleFor(dl => dl.Id, 0)
                .RuleFor(dl => dl.Category, f => Constants.DRIVERSLICENCE_CATEGORIES.ElementAt(index))
                .RuleFor(dl => dl.Description, f => Constants.DRIVERSLICENCE_CATEGORIES_DESCRIPTIONS.ElementAt(index));
        }
    }
}
