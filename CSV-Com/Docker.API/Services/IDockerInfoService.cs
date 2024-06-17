
namespace Docker.API.Services
{
    public interface IDockerInfoService
    {
        Task<string> GetContainerLogs(string containerName, CancellationToken? token = null);
        Task<string> GetContainerProcesses(CancellationToken? token = null);
    }
}
