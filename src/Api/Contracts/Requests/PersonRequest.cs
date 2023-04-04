﻿namespace DockerTestsSample.Api.Contracts.Requests;

public sealed class PersonRequest
{
    public required string Name { get; init; }

    public required string LastName { get; init; }
    
    public required DateOnly BirthDate { get; init; }
    
    public string? Email { get; set; }
}
