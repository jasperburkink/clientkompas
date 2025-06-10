namespace Docker.API.Services
{
    public interface IDockerRefreshService
    {
        string? CheckRefreshJob(string identifier);
        string? GetJobResult(string identifier);
        string RefreshEnvironments();
    }
}
