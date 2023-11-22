using System.Reflection;

namespace ArchitectureTests
{
    public class BaseTestFixture : IDisposable
    {
        private readonly List<Assembly> _assemblies;

        public BaseTestFixture()
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

        public Assembly GetAssemblyByName(string name)
        {
            var assymbly = _assemblies.FirstOrDefault(assembly => assembly.GetName().Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (assymbly == null)
            {
                assymbly = Assembly.Load(name);
                _assemblies.Add(assymbly);
            }

            return assymbly;
        }

        public void Dispose()
        {

        }
    }
}
