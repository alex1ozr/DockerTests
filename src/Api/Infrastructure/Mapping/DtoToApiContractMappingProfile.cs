using AutoMapper;
using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Services.People;
using JetBrains.Annotations;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

[UsedImplicitly]
internal sealed class DtoToApiContractMappingProfile : Profile
{
    public DtoToApiContractMappingProfile()
    {
        CreateMap<PersonDto, PersonResponse>();
    }
}