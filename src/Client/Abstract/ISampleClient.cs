using DockerTestsSample.Client.Implementations;

namespace DockerTestsSample.Client.Abstract;

/// <summary>
/// Sample API client
/// </summary>
public interface ISampleClient
{
    /// <summary>
    /// People API client
    /// </summary>
    IPersonClient People { get; }
}
