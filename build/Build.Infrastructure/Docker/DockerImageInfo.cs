namespace DockerTestsSample.Build.Infrastructure.Docker;

public interface IDockerImageInfo
{
    string DockerImageName { get; }
}

public record class DockerImageInfo(
    string DockerImageName,
    string DockerfileName) : IDockerImageInfo;

public record class GeneratedDockerImageInfo(
    string DockerImageName, string ProjectName) : IDockerImageInfo;