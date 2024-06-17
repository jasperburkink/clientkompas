using Docker.API.Options;
using Docker.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Docker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DockerInfoController : ControllerBase
    {
        private readonly ILogger<DockerInfoController> _logger;
        private readonly IDockerInfoService _dockerInfoService;
        private readonly IOptions<SecurityOptions> _options;

        public DockerInfoController(ILogger<DockerInfoController> logger, IDockerInfoService dockerInfoService, IOptions<SecurityOptions> options)
        {
            _logger = logger;
            _dockerInfoService = dockerInfoService;
            _options = options;
        }

        [HttpGet("get_processes")]
        public async Task<ActionResult<string>> GetProcesses()
        {
            try
            {
                return await _dockerInfoService.GetContainerProcesses();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Getting processes failed: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get_container_logs")]
        public async Task<ActionResult<string>> GetContainerLogs(string container)
        {
            try
            {
                return await _dockerInfoService.GetContainerLogs(container);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Getting processes failed: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
