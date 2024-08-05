using System.Globalization;
using System.Reflection;
using System.Resources;
using Application.Common.Interfaces;

namespace Application.Common.Resources
{
    public class ResourceMessageProvider : IResourceMessageProvider
    {
        public CultureInfo CultureInfo { get; private set; }

        public ResourceMessageProvider(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public string GetMessage<T>(string key, params object[] args)
        {
            var type = typeof(T);
            return GetMessage(type, key, args);
        }

        public string GetMessage(Type type, string key, params object[] args)
        {
            var namespaceName = type.Namespace;

            var resourceManager = new ResourceManager($"{namespaceName}.Resources.{type.Name}", Assembly.GetExecutingAssembly());

            var message = resourceManager.GetString(key, CultureInfo.CurrentUICulture);
            return args.Length > 0 ? string.Format(message, args) : message;
        }
    }
}
