using AutoMapper;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Api.Mapping;

public sealed class ApiContractToDtoMappingProfile: Profile
{
    public ApiContractToDtoMappingProfile()
    {
        CreateMap<PersonRequest, PersonDto>();
        CreateMap<UpdatePersonRequest, PersonDto>();
    }
}