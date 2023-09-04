using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Contracts.Requests;

public sealed class UpdatePersonRequest
{
    [FromRoute(Name = "id")] 
    public required Guid Id { get; set; }

    [FromBody] 
    public required PersonRequest Person { get; set; }
}
