using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Infrastructure.Problems;

/// <summary>
/// Default values for <see cref="ProblemDetails.Type"/>.
/// </summary>
internal static class DefaultProblemDetailTypes
{
    /// <summary>
    /// Value for <see cref="ProblemDetails.Type"/> if validation error has occured.
    /// </summary>
    public const string ValidationProblem = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";

    /// <summary>
    /// Value for <see cref="ProblemDetails.Type"/> if unhandled error has occured.
    /// </summary>
    /// <remarks>
    /// See also https://datatracker.ietf.org/doc/html/rfc7807#section-4.2
    /// </remarks>
    public const string UnhandledException = "about:blank";

    /// <summary>
    /// Value for <see cref="ProblemDetails.Type"/> if business logic related error has occured.
    /// </summary>
    /// <remarks>
    /// Used if only <see cref="IErrorDescription.ErrorCode"/> is null or empty.
    /// </remarks>
    public const string BusinessLogicException = "https://datatracker.ietf.org/doc/html/rfc7807";
}
