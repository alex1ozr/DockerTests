using Nuke.Common;
using Serilog;

namespace DockerTestsSample.Build.Infrastructure.BuildComponents;

public interface IReleaseBuild : IBaseBuild
{
    Target CreateRelease => _ => _
        .TryDependsOn<INuGetBuild>(x => x.PushNuGetArtifacts)
        .TryDependsOn<IDockerBuild>(x => x.PushDockerArtifacts)
        .Executes(() =>
        {
            // Put here any logic you need to create release using your CI/CD system API

            Log.Information("Release {ReleaseName} was successfully created for service {ServiceName}",
                Version.FullVersion, ServiceName);
        });
}