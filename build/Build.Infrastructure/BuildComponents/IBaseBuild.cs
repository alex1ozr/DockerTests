using DockerTestsSample.Build.Infrastructure.Common;
using DockerTestsSample.Build.Infrastructure.Extensions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace DockerTestsSample.Build.Infrastructure.BuildComponents;

public interface IBaseBuild : INukeBuild
{
    [Solution]
    Solution Solution => this.GetValue(() => Solution);

    [Parameter("Build counter"), Required]
    string BuildCounter => this.GetValue(() => BuildCounter);

    [Parameter("Branch name"), Required]
    string Branch => this.GetValue(() => Branch);

    string ServiceName { get; }

    ApplicationVersion Version { get; }

    AbsolutePath ArtifactsPath => RootDirectory / ".artifacts";

    AbsolutePath BuildPath => RootDirectory / "build" / "Build";
}
