using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

internal static class ApiToDtoMappingExtensions
{
    public static PersonDto ToPersonDto(this PersonRequest personRequest, Guid id)
        => new()
        {
            Id = id,
            Name = personRequest.Name,
            LastName = personRequest.LastName,
            BirthDate = personRequest.BirthDate,
            Email = personRequest.Email
        };
}