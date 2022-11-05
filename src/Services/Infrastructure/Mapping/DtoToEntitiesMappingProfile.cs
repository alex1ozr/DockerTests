using AutoMapper;
using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Services.Infrastructure.Mapping;

public class DtoToEntitiesMappingProfile: Profile
{
    public DtoToEntitiesMappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<PersonDto, Person>();
    }
}