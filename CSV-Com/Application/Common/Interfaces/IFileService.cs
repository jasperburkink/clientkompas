namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        string ReadAllText(string path);

        void WriteAllText(string path, string content);

        bool Exists(string path);

        void Delete(string path);
    }
}
