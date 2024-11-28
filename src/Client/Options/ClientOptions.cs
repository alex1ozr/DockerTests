using System.ComponentModel.DataAnnotations;

namespace DockerTestsSample.Client.Options;

public sealed class ClientOptions
{
    public const string OptionKey = "SampleApi";

    private const string EmptyServerUrlMessage = $"Configuration value '{OptionKey}:{nameof(ServerUrl)}' is not set.";
    
    [Required]
    public Uri? ServerUrl { get; set; }

    public Uri RequiredServerUrl => ServerUrl ?? throw new ArgumentNullException(nameof(ServerUrl), EmptyServerUrlMessage);
}
