using AutoMapper;
using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Services.Infrastructure.Mapping;

public sealed class DtoToEntitiesMappingProfile: Profile
{
    public DtoToEntitiesMappingProfile()
    {
        CreateMap<Person, PersonDto>()
            .ReverseMap();
    }
}