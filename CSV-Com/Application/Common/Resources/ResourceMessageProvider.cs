using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Application.Common.Interfaces;

namespace Application.Common.Resources
{
    public class ResourceMessageProvider(CultureInfo cultureInfo) : IResourceMessageProvider
    {
        private readonly ConcurrentDictionary<string, string> _resourceCache = new();
        public CultureInfo CultureInfo { get; private set; } = cultureInfo;

        public void LoadResources(Type type)
        {
            var namespaceName = type.Namespace;

            var resourceManager = new ResourceManager($"{namespaceName}.Resources.{type.Name}", Assembly.GetExecutingAssembly());

            var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            foreach (var entry in resourceSet)
            {
                if (entry is DictionaryEntry resourceEntry)
                {
                    _resourceCache.TryAdd($"{type.FullName}.{resourceEntry.Key}", resourceEntry.Value.ToString()!);
                }
            }
        }

        public string GetMessage<T>(string key, params object[] args)
        {
            var type = typeof(T);
            return GetMessage(type, key, args);
        }

        public string GetMessage(Type type, string key, params object[] args)
        {
            if (!_resourceCache.Any(r => r.Key.StartsWith(type.FullName)))
            {
                LoadResources(type);
            }

            var cacheKey = $"{type.FullName}.{key}";

            if (_resourceCache.TryGetValue(cacheKey, out var message))
            {
                return args.Length > 0 ? string.Format(message, args) : message;
            }

            Debug.WriteLine($"Resource key '{key}' not found for type '{type.FullName}'.");
            return string.Empty;
        }
    }
}
