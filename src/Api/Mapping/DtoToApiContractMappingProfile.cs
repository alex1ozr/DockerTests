using AutoMapper;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Api.Mapping;

public sealed class DtoToApiContractMappingProfile: Profile
{
    public DtoToApiContractMappingProfile()
    {
        CreateMap<PersonDto, PersonResponse>();
    }
}