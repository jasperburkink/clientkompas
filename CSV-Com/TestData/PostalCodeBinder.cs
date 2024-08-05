using System.Reflection;
using System.Text.RegularExpressions;
using Bogus;

namespace TestData
{
    public class PostalCodeBinder : IAutoBinder
    {
        public TType CreateInstance<TType>(AutoGenerateContext context)
        {
            if (typeof(TType) == typeof(string))
            {
                var validPostalCodeRegex = new Regex(Properties.Resources.PostalCodeRegex);
                string zipCode;
                do
                {
                    var faker = new Faker(FakerConfiguration.Localization);
                    zipCode = faker.Address.ZipCode();
                } while (!validPostalCodeRegex.IsMatch(zipCode));
                return (TType)(object)zipCode;
            }
            return Activator.CreateInstance<TType>();
        }

        public Dictionary<string, MemberInfo> GetMembers(Type t)
        {
            return new Dictionary<string, MemberInfo>();
        }

        public void PopulateInstance<TType>(object instance, AutoGenerateContext context, IEnumerable<MemberInfo> members = null)
        {
        }
    }
}
