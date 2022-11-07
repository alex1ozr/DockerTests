using AutoMapper;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

public sealed class DtoToApiContractMappingProfile: Profile
{
    public DtoToApiContractMappingProfile()
    {
        CreateMap<PersonDto, PersonResponse>();
    }
}