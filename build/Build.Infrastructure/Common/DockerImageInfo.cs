namespace DockerTestsSample.Build.Infrastructure.Common;

public record class DockerImageInfo(
    string DockerImageName,
    string DockerfileName);