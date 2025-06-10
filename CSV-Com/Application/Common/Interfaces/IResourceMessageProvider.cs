using System.Globalization;

namespace Application.Common.Interfaces
{
    public interface IResourceMessageProvider
    {
        CultureInfo CultureInfo { get; }

        string GetMessage(Type type, string key, params object[] args);

        string GetMessage<T>(string key, params object[] args);
    }
}
