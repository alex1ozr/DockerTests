using DockerTestsSample.Build.Infrastructure.BuildComponents;
using DockerTestsSample.Build.Infrastructure.Common;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace DockerTestsSample.Build.Infrastructure.Extensions;

internal static class IDockerBuildExtensions
{
    public static void BuildDockerfile(
        this IDockerBuild build,
        DockerImageInfo dockerImageInfo)
    {
        Log.Information("Building Docker image {DockerImageName} ({DockerfileName})",
            dockerImageInfo.DockerImageName, dockerImageInfo.DockerfileName);

        var dockerfilePath = build.BuildPath / dockerImageInfo.DockerfileName;

        DockerBuild(settings =>
        {
            var dockerBuildSettings = settings
                .SetPath(build.RootDirectory)
                .SetFile(dockerfilePath)
                .SetTag(build.GetDockerImageTag(dockerImageInfo.DockerImageName))
                .EnablePull()
                .SetProgress(ProgressType.plain)
                .SetTarget("final")
                .SetBuildArg(
                    $"Version={build.Version.FullVersion}",
                    $"AssemblyVersion={build.Version.AssemblyVersion}",
                    $"FileVersion={build.Version.FileVersion}",
                    $"InformationalVersion={build.Version.InformationalVersion}");

            return dockerBuildSettings;
        });
    }

    public static string CreateDockerContainer(this IDockerBuild build, string dockerImageName)
    {
        Log.Information("Creating Docker container for {DockerImageName}", dockerImageName);
        var createResult = DockerContainerCreate(settings => settings.SetImage(build.GetDockerImageTag(dockerImageName)));

        Assert.Count(createResult, 1);
        Assert.True(createResult.Single().Type == OutputType.Std);
        var containerId = createResult.Single().Text;
        containerId.NotNullOrWhiteSpace();

        return containerId;
    }

    public static void CopyArtifactsFromContainer(this IDockerBuild build, string containerId)
    {
        Log.Information("Copying items from Docker container {ContainerId}", containerId);

        var source = $"{IDockerBuild.DockerContainerArtifactsPath}/.";
        var destination = build.ArtifactsPath;

        var containerSource = $"{containerId}:{source}";
        Docker($"container cp {containerSource} {destination}");
    }

    public static void RemoveDockerContainer(this IDockerBuild build, string containerId)
    {
        Log.Information("Removing Docker container {ContainerId}", containerId);

        var removeResult = DockerContainerRm(settings => settings
            .SetContainers(containerId)
            .EnableForce());

        Assert.Count(removeResult, 1);
        Assert.True(string.Equals(removeResult.Single().Text, containerId, StringComparison.OrdinalIgnoreCase));
    }
}
