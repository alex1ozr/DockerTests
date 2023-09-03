using DockerTestsSample.Client.Abstract;

namespace DockerTestsSample.Client.Implementations;

internal sealed class SampleClient: ISampleClient
{
    public SampleClient(IPersonClient personClient)
    {
        People = personClient;
    }

    public IPersonClient People { get; }
}
