using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Contracts.Requests;

public sealed class CreatePersonRequest
{
    [FromRoute(Name = "id")] 
    public Guid Id { get; init; }

    [FromBody] 
    public PersonRequest Person { get; set; } = default!;
}
