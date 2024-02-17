using Scriban.Runtime;

namespace DockerTestsSample.Build.Infrastructure.Docker;

public sealed class DockerfileModel : ScriptObject
{
    public DockerfileModel(
        string projectToPublish,
        string assemblyName,
        bool publishArtifacts)
    {
        Add("ProjectToPublish", projectToPublish);
        Add("AssemblyName", assemblyName);
        Add("PublishArtifacts", publishArtifacts.ToString().ToLower());
    }
}