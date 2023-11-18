using AutoMapper;
using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Services.Dto;
using JetBrains.Annotations;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

[UsedImplicitly]
internal sealed class ApiContractToDtoMappingProfile: Profile
{
    public ApiContractToDtoMappingProfile()
    {
        CreateMap<PersonRequest, PersonDto>()
            .ForMember(d => d.Id, c => c.Ignore());
    }
}