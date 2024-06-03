using Docker.API.Controllers;
using Docker.API.Helpers;

namespace Docker.API.Services
{
    public class DockerRefreshService : IDockerRefreshService
    {
        private readonly ILogger<DockerInternalController> _logger;
        private readonly SemaphoreSlim _semaphore = new(1);
        private readonly Dictionary<string, Task<ExecutionResult>> _callStatusMap = new();

        public DockerRefreshService(ILogger<DockerInternalController> logger)
        {
            _logger = logger;
        }

        public string RefreshEnvironments()
        {
            var identifier = Guid.NewGuid().ToString();

            _logger.LogInformation("Starting refresh with identifier: " + identifier);

            _ = InternalRefreshEnvironments(identifier);

            return identifier;
        }

        public string? CheckRefreshJob(string identifier)
        {
            if (!_callStatusMap.ContainsKey(identifier))
            {
                return null;
            }

            var task = _callStatusMap[identifier];

            if (!task.IsCompleted)
            {
                return "Loading";
            }

            return task.Result.Success ? "Success" : "Error";

        }

        public string? GetJobResult(string identifier)
        {
            if (!_callStatusMap.ContainsKey(identifier))
            {
                return null;
            }

            var task = _callStatusMap[identifier];

            if (!task.IsCompleted)
            {
                return "Loading";
            }

            return task.Result.ToString();
        }

        private async Task InternalRefreshEnvironments(string identifier) => await Task.Run(async () =>
        {
            _ = await _semaphore.WaitAsync(300_000);
            var tokenSource = new CancellationTokenSource(300_000);

            try
            {
                var task = ExecutionHelper.ExecuteAsync("/var/docker/pull-latest-version", tokenSource.Token);
                _callStatusMap.Add(identifier, task);
                var result = await task;
                _logger.LogInformation($"Got task result for identifier {identifier}. Succes: {result.Success}. Error: {result.Error}, Message: {result.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Task {identifier} failed");
            }
            finally
            {
                _ = _semaphore.Release();
            }
        });
    }
}
