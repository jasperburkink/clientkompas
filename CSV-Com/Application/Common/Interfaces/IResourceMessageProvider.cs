using System.Globalization;

namespace Application.Common.Interfaces
{
    public interface IResourceMessageProvider
    {
        CultureInfo CultureInfo { get; }

        string GetMessage<T>(string key, params object[] args);
    }
}
