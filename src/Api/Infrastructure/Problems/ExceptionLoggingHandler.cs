using DockerTestsSample.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DockerTestsSample.Api.Infrastructure.Problems;

/// <summary>
/// Exception handler that logs exceptions using the provided logger.
/// </summary>
internal sealed class ExceptionLoggingHandler(ILogger<ExceptionLoggingHandler> logger) : IExceptionHandler
{
    private readonly ILogger _logger = logger;

    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        const string message = "An unhandled exception has occurred while executing the request";

        if (exception is DomainException)
        {
            _logger.LogWarning(exception, message);
        }
        else
        {
            _logger.LogError(exception, message);
        }

        // Always return false to allow other handlers to process the exception
        return ValueTask.FromResult(false);
    }
}
