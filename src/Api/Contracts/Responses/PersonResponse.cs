namespace DockerTestsSample.Api.Contracts.Responses;

public sealed class PersonResponse
{
    public Guid Id { get; init; }

    public string Name { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public int Age { get; set; }
    
    public string? Email { get; set; }
}
