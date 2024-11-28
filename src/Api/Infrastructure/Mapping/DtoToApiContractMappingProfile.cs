using AutoMapper;
using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Services.People;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

internal sealed class DtoToApiContractMappingProfile : Profile
{
    public DtoToApiContractMappingProfile()
    {
        CreateMap<PersonDto, PersonResponse>();
    }
}