using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Contracts.Requests;

public sealed class CreatePersonRequest
{
    [FromRoute(Name = "id")] 
    public required Guid Id { get; init; }

    [FromBody] 
    public required PersonRequest Person { get; init; }
}
