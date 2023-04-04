namespace DockerTestsSample.PopulationDbContext.Entities;

public sealed class Person
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string LastName { get; init; }

    public required DateOnly BirthDate { get; init; }

    public string? Email { get; set; }
}