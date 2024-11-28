using DockerTestsSample.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Infrastructure.Problems;

public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };
        
        if (exception is DomainException validationException)
        {
            problemDetails.Title = validationException.ShortDescription;
            problemDetails.Type = validationException.ErrorCode;
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}