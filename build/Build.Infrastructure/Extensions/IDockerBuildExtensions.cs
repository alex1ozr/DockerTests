using DockerTestsSample.Build.Infrastructure.BuildComponents;
using DockerTestsSample.Build.Infrastructure.Docker;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;
using static Nuke.Common.Tools.Docker.DockerTasks;

namespace DockerTestsSample.Build.Infrastructure.Extensions;

internal static class IDockerBuildExtensions
{
    public static void BuildDockerfile(
        this IDockerBuild build,
        IDockerImageInfo dockerImageInfo)
    {
        switch (dockerImageInfo)
        {
            case DockerImageInfo fileDockerImageInfo:
                build.BuildDockerfile(fileDockerImageInfo);
                break;
            case GeneratedDockerImageInfo generatedDockerImageInfo:
                build.BuildDockerfile(generatedDockerImageInfo);
                break;
            default:
                throw new NotSupportedException($"Docker image info type {dockerImageInfo.GetType()} is not supported");
        }
    }

    public static string CreateDockerContainer(this IDockerBuild build, string dockerImageName)
    {
        Log.Information("Creating Docker container for {DockerImageName}", dockerImageName);
        var createResult =
            DockerContainerCreate(settings => settings.SetImage(build.GetDockerImageTag(dockerImageName)));

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
        DockerTasks.Docker($"container cp {containerSource} {destination}");
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

    private static void BuildDockerfile(
        this IDockerBuild build,
        DockerImageInfo dockerImageInfo)
    {
        Log.Information("Building Docker image {DockerImageName} ({DockerfileName})",
            dockerImageInfo.DockerImageName, dockerImageInfo.DockerfileName);

        var dockerfilePath = build.BuildPath / dockerImageInfo.DockerfileName;

        RunDockerBuild(build, dockerImageInfo, dockerfilePath);
    }

    private static void BuildDockerfile(
        this IDockerBuild build,
        GeneratedDockerImageInfo dockerImageInfo)
    {
        Log.Information("Building Docker image {DockerImageName} from generated Dockerfile",
            dockerImageInfo.DockerImageName);

        var project = build.Solution.AllProjects
                          .SingleOrDefault(x =>
                              string.Equals(x.Name, dockerImageInfo.ProjectName, StringComparison.Ordinal))
                      ?? throw new InvalidOperationException($"Project {dockerImageInfo.ProjectName} not found");

        var dockerfilePath = DockerfileGenerator.GenerateDockerfile(project);
        Log.Information("Dockerfile ({DockerfilePath}) was generated", dockerfilePath);

        RunDockerBuild(build, dockerImageInfo, dockerfilePath);

        dockerfilePath.DeleteFile();
        Log.Information("Dockerfile ({DockerfilePath}) was deleted", dockerfilePath);
    }

    private static void RunDockerBuild(
        IDockerBuild build,
        IDockerImageInfo dockerImageInfo,
        AbsolutePath dockerfilePath)
    {
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
}
