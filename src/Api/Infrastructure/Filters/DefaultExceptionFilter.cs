using System.Text.Json;
using DockerTestsSample.Api.Infrastructure.Problems;
using DockerTestsSample.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DockerTestsSample.Api.Infrastructure.Filters;

internal sealed class DefaultExceptionFilter : IExceptionFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger _logger;

    public DefaultExceptionFilter(
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<DefaultExceptionFilter> logger)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not DomainException serviceException)
        {
            // Handle this exception as server or infrastructure related
            var commonProblemDetails = _problemDetailsFactory.CreateProblemDetails(context.HttpContext, detail: context.Exception.Message);
            SetStatusAndResponse(context, commonProblemDetails);

            LogProblem(commonProblemDetails, isError: true);
            return;
        }

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            StatusCodes.Status422UnprocessableEntity,
            string.IsNullOrWhiteSpace(serviceException.ShortDescription)
                ? "Unable to perform an operation"
                : serviceException.ShortDescription,
            string.IsNullOrWhiteSpace(serviceException.ErrorCode)
                ? DefaultProblemDetailTypes.BusinessLogicException
                : serviceException.ErrorCode,
            serviceException.Message,
            instance: null
        );

        var statusCode = serviceException switch
        {
            PersonAlreadyExistsException => StatusCodes.Status409Conflict,
            PersonNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status422UnprocessableEntity
        };

        SetStatusAndResponse(context, problemDetails, statusCode);
        LogProblem(problemDetails, isError: false);
    }

    private static void SetStatusAndResponse(ExceptionContext context, ProblemDetails problemDetails, int? overridenStatusCode = null)
    {
        problemDetails.Status = overridenStatusCode ?? problemDetails.Status;

        if (problemDetails.Status.HasValue)
        {
            context.HttpContext.Response.StatusCode = problemDetails.Status.Value;
        }

        context.Result = new ObjectResult(problemDetails) { StatusCode = overridenStatusCode };

        context.ExceptionHandled = true;
    }

    private void LogProblem(ProblemDetails problemDetails, bool isError)
    {
        const string message = "An error occurred while processing your request. Uri: {RequestUri}. Message: {ErrorMessage}. {Model}";

        if (isError)
        {
            _logger.LogError(message, problemDetails.Instance, problemDetails.Detail, JsonSerializer.Serialize(problemDetails));
        }
        else
        {
            _logger.LogWarning(message, problemDetails.Instance, problemDetails.Detail, problemDetails);
        }
    }
}