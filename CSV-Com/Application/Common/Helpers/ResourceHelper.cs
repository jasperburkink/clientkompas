using System.Diagnostics;
using System.Reflection;

namespace Application.Common.Helpers
{
    internal static class ResourceHelper
    {
        public static IEnumerable<Type> GetResourceTypes(Assembly assembly)
        {
            // Get all .resources filenames in the assembly
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(name => name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var resourceName in resourceNames)
            {
                var className = resourceName.Replace(".resources", string.Empty, StringComparison.OrdinalIgnoreCase);//.Replace($"{assembly.GetName().Name}.", string.Empty, StringComparison.OrdinalIgnoreCase);

                var resourceType = assembly.GetType(className);

                if (resourceType != null)
                {
                    yield return resourceType;
                }
                else
                {
                    Debug.WriteLine($"Resource type for '{className}' not found.");
                }
            }
        }
    }
}
