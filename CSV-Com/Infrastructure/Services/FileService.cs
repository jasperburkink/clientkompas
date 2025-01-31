using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        public string ReadAllText(string path) => File.ReadAllText(path);

        public void WriteAllText(string path, string content) => File.WriteAllText(path, content);

        public bool Exists(string path) => File.Exists(path);

        public void Delete(string path) => File.Delete(path);
    }
}
