using Docker.API.Helpers;

namespace Docker.API.Services
{
    public class DockerInfoService : IDockerInfoService
    {
        public async Task<string> GetContainerLogs(string containerName, CancellationToken? token = null)
        {
            var command = $"logs \"{containerName}\"";
            var response = await ExecutionHelper.ExecuteDockerCommand(command, token);
            return response;
        }

        public async Task<string> GetContainerProcesses(CancellationToken? token = null)
        {
            var command = "ps";
            var response = await ExecutionHelper.ExecuteDockerCommand(command, token);
            return response;
        }
    }
}
