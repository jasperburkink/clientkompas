using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests
{
    public class BaseTestFixture : IDisposable
    {
        private List<Assembly> _assemblies;

        public BaseTestFixture()
        {
            _assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        }

        public Assembly GetAssemblyByName(string name)
        {
            var assymbly = _assemblies.FirstOrDefault(assembly => assembly.GetName().Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if(assymbly == null)
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
