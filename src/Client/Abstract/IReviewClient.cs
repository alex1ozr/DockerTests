using DockerTestsSample.Client.Implementations;

namespace DockerTestsSample.Client.Abstract;

public interface ISampleClient
{
    IPersonClient People { get; }
}
