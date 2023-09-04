using System.ComponentModel.DataAnnotations;

namespace DockerTestsSample.Client.Options;

public sealed class ClientOptions
{
    public const string OptionKey = "SampleApi";

    private const string UrlUnsetError = $"Configuration value '{OptionKey}.{nameof(ServerUrl)}' is not set.";

    public Uri Url() => ServerUrl ?? throw new ArgumentNullException(nameof(ServerUrl), UrlUnsetError);

    [Required]
    private Uri? ServerUrl { get; set; }
}
