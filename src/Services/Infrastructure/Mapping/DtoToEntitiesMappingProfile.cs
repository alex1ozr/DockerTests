using AutoMapper;
using DockerTestsSample.Services.Dto;
using DockerTestsSample.Store.Entities;

namespace DockerTestsSample.Services.Infrastructure.Mapping;

public sealed class DtoToEntitiesMappingProfile: Profile
{
    public DtoToEntitiesMappingProfile()
    {
        CreateMap<Person, PersonDto>()
            .ReverseMap();
    }
}