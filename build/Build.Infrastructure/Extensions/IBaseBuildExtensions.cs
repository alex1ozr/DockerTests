using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DockerTestsSample.Build.Infrastructure.BuildComponents;

namespace DockerTestsSample.Build.Infrastructure.Extensions;

internal static class IBaseBuildExtensions
{
    private static readonly Regex PreReleaseSuffixRegex = new("[^0-9A-Za-z-]",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static string GetPreReleaseSuffix(this IBaseBuild build)
    {
        var branch = build.Branch;
        if (string.Equals(branch, "main", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        var suffix = PreReleaseSuffixRegex.Replace(branch, "-");
        return $"-{suffix}";
    }
    
    public static T GetValue<T>(this IBaseBuild build, Expression<Func<T>> parameterExpression)
        where T : class
        => build.TryGetValue(parameterExpression)
           ?? throw new InvalidOperationException($"Cannot get value for {parameterExpression}");
}
