using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace DockerTestsSample.Build.Infrastructure.BuildComponents;

public interface IIntegrationTestsBuild : IBaseBuild
{
    public const string TestsCategory = "IntegrationTests";

    bool ExecuteIntegrationTests => false;

    Target RunIntegrationTests => _ => _
        .TryDependsOn<IDockerBuild>(x => x.BuildDockerfileWithArtifacts)
        .OnlyWhenDynamic(() => ExecuteIntegrationTests)
        .WhenSkipped(DependencyBehavior.Execute)
        .Executes(() =>
        {
            DotNetBuild(settings =>
                settings
                    .SetProjectFile(Solution)
                    .SetConfiguration("Release")
                    .SetVerbosity(DotNetVerbosity.quiet)
                    .EnableNoLogo());

            return DotNetTest(settings =>
                settings
                    .SetProjectFile(Solution)
                    .SetFilter($"Category={TestsCategory}")
                    .SetConfiguration("Release")
                    .SetVerbosity(DotNetVerbosity.normal)
                    .SetLoggers("console;verbosity=normal")
                    .EnableNoLogo()
                    .EnableNoRestore()
                    .EnableNoBuild());
        });
}
