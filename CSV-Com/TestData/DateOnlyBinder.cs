using System.Reflection;

namespace TestData
{
    internal class DateOnlyBinder : IAutoBinder
    {
        public TType CreateInstance<TType>(AutoGenerateContext context)
        {
            if (typeof(TType) == typeof(DateOnly))
            {
                var random = new Random();
                var randomDate = new DateTime(random.Next(1960, 2020), random.Next(1, 12), random.Next(1, 28));
                return (TType)(object)DateOnly.FromDateTime(randomDate); // Converteer naar DateOnly
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
