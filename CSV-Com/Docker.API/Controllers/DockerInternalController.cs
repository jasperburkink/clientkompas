using Docker.API.Options;
using Docker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Docker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DockerInternalController : ControllerBase
    {
        private const int ShutdownDelayMilliseconds = 500;
        private readonly ILogger<DockerInternalController> _logger;
        private readonly IDockerRefreshService _dockerRefreshService;
        private readonly IDelayedKillService _delayedKillService;
        private readonly IOptions<SecurityOptions> _options;

        public DockerInternalController(ILogger<DockerInternalController> logger, IDockerRefreshService dockerRefreshService, IDelayedKillService delayedKillService, IOptions<SecurityOptions> options)
        {
            _logger = logger;
            _dockerRefreshService = dockerRefreshService;
            _delayedKillService = delayedKillService;
            _options = options;
        }

        [HttpGet("refresh_environments")]
        public ActionResult<string> RefreshEnvironments(string s)
        {
            if (!IsAuthorized(s))
            {
                _logger.LogWarning("Unauthorized call denied. Secret: " + s);
                return Unauthorized();
            }

            try
            {
                return _dockerRefreshService.RefreshEnvironments();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Job failed: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("check_job")]
        public ActionResult<string> CheckRefreshJob(string identifier)
        {
            var result = _dockerRefreshService.CheckRefreshJob(identifier);

            if (result is null)
            {
                _logger.LogInformation($"Status of job: {identifier} was not found");
                return NotFound();
            }

            _logger.LogInformation($"Status of job: {identifier} was {result}");
            return Ok(result);

        }

        [HttpGet("job_result")]
        public ActionResult<string> GetJobResult(string identifier)
        {
            var result = _dockerRefreshService.GetJobResult(identifier);

            if (result is null)
            {
                _logger.LogInformation($"Status of job: {identifier} was not found");
                return NotFound();
            }

            _logger.LogInformation($"Status of job: {identifier} was {result}");
            return Ok(result);
        }

        [HttpGet("exit")]
        public ActionResult Exit(string s)
        {
            if (!IsAuthorized(s))
            {
                _logger.LogWarning($"Unauthorized call denied. Secret: {s}");
                return Unauthorized();
            }

            _delayedKillService.KillApplicationDelayed(ShutdownDelayMilliseconds);

            return Ok("Exiting");
        }

        private bool IsAuthorized(string secret)
        {
            var configuredSecret = _options.Value.Secret;
            return secret == configuredSecret;
        }
    }
}
