using DockerTestsSample.Build.Infrastructure.BuildComponents;
using DockerTestsSample.Build.Infrastructure.Common;

namespace DockerTestsSample.Build.Infrastructure.Extensions;

public static class BuildVersioningExtensions
{
    private static ApplicationVersion? _versionCache;

    /// <summary>
    /// Use semantic versioning according to https://semver.org/
    /// </summary>
    public static ApplicationVersion UseSemanticVersion(this IBaseBuild build, int major, int minor)
    {
        if (_versionCache is not null)
        {
            return _versionCache;
        }

        var preReleaseSuffix = build.GetPreReleaseSuffix();
        _versionCache = new(
            FullVersion: $"{major}.{minor}.{build.BuildCounter}{preReleaseSuffix}",
            AssemblyVersion: $"{major}.{minor}",
            FileVersion: $"{major}.{minor}.{build.BuildCounter}",
            InformationalVersion: $"{major}.{minor}.{build.BuildCounter}{preReleaseSuffix}");

        return _versionCache;
    }
}
