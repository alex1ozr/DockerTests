using System.Collections.Generic;
using DockerTestsSample.Build.Infrastructure;
using DockerTestsSample.Build.Infrastructure.Common;
using DockerTestsSample.Build.Infrastructure.Docker;
using DockerTestsSample.Build.Infrastructure.Extensions;
using Nuke.Common;

class Build : NukeBuild, IDefaultBuildFlow
{
    public string ServiceName => "DockerTestsSample";

    public ApplicationVersion Version => this.UseSemanticVersion(major: 1, minor: 0);

    public bool ExecuteIntegrationTests => true;

    public IReadOnlyList<IDockerImageInfo> DockerImages { get; } = new[]
    {
        new GeneratedDockerImageInfo(DockerImageName: "docker-tests-sample", ProjectName: "Api"),

        // Uncomment this line to build a Docker image from an existing Dockerfile:
        //new DockerImageInfo(DockerImageName: "docker-tests-sample", DockerfileName: "Dockerfile"),
    };

    public static int Main()
        => Execute<Build>(x => ((IDefaultBuildFlow)x).Default);
}
