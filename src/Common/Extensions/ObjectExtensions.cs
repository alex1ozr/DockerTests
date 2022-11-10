using System.Runtime.CompilerServices;

namespace DockerTestsSample.Common.Extensions;

public static class ObjectExtensions
{
    public static T Required<T>(this T? value, [CallerArgumentExpression("value")] string? paramName = default)
        => value ?? throw new ArgumentNullException(paramName);
}