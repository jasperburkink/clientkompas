using Docker.API.Options;
using Docker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Docker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DockerInternalController(ILogger<DockerInternalController> logger, IDockerRefreshService dockerRefreshService, IDelayedKillService delayedKillService, IOptions<SecurityOptions> options) : ControllerBase
    {
        private const int ShutdownDelayMilliseconds = 500;

        [HttpGet("refresh_environments")]
        public ActionResult<string> RefreshEnvironments(string s)
        {
            if (!IsAuthorized(s))
            {
                logger.LogWarning("Unauthorized call denied. Secret: " + s);
                return Unauthorized();
            }

            try
            {
                return dockerRefreshService.RefreshEnvironments();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Job failed: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("check_job")]
        public ActionResult<string> CheckRefreshJob(string identifier)
        {
            var result = dockerRefreshService.CheckRefreshJob(identifier);

            if (result is null)
            {
                logger.LogInformation($"Status of job: {identifier} was not found");
                return NotFound();
            }

            logger.LogInformation($"Status of job: {identifier} was {result}");
            return Ok(result);

        }

        [HttpGet("job_result")]
        public ActionResult<string> GetJobResult(string identifier)
        {
            var result = dockerRefreshService.GetJobResult(identifier);

            if (result is null)
            {
                logger.LogInformation($"Status of job: {identifier} was not found");
                return NotFound();
            }

            logger.LogInformation($"Status of job: {identifier} was {result}");
            return Ok(result);
        }

        [HttpGet("exit")]
        public ActionResult Exit(string s)
        {
            if (!IsAuthorized(s))
            {
                logger.LogWarning($"Unauthorized call denied. Secret: {s}");
                return Unauthorized();
            }

            delayedKillService.KillApplicationDelayed(ShutdownDelayMilliseconds);

            return Ok("Exiting");
        }

        private bool IsAuthorized(string secret)
        {
            var configuredSecret = options.Value.Secret;
            return secret == configuredSecret;
        }
    }
}
