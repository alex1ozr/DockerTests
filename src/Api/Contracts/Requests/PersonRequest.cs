namespace DockerTestsSample.Api.Contracts.Requests;

public sealed class PersonRequest
{
    public string Name { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateTime BirthDate { get; set; }
    
    public string? Email { get; set; }
}
