using Microsoft.AspNetCore.Diagnostics;

namespace Docker.API.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is BadImageFormatException badImageFormatException)
            {
                logger.LogError(exception, "Got a BadImageFormatException, exiting application");
                Environment.Exit(0);
                return ValueTask.FromResult(true);
            }

            return ValueTask.FromResult(false);
        }
    }
}
