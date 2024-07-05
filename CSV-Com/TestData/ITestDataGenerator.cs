using Bogus;

namespace TestData
{
    public interface ITestDataGenerator<T> where T : class
    {
        public Faker<T> Faker { get; }

        public T Create()
        {
            return Faker.Generate();
        }

        public ICollection<T> Create(int count = 1)
        {
            var faker = Faker;

            return faker.Generate(count);
        }
    }
}
