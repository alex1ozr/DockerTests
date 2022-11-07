using AutoMapper;
using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.Services.Dto;

namespace DockerTestsSample.Services.Infrastructure.Mapping;

public sealed class DtoToEntitiesMappingProfile: Profile
{
    public DtoToEntitiesMappingProfile()
    {
        CreateMap<Person, PersonDto>()
            .ForCtorParam("id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("name", o => o.MapFrom(s => s.Name))
            .ForCtorParam("lastName", o => o.MapFrom(s => s.LastName))
            .ForCtorParam("birthDate", o => o.MapFrom(s => s.BirthDate))
            .ForMember(d => d.Email,
                c => c.MapFrom(s => s.Email))
            .AfterMap((person, _) =>
            {
                person.BirthDate = person.BirthDate.ToUniversalTime();
            });
        
        CreateMap<PersonDto, Person>()
            .ForCtorParam("id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("name", o => o.MapFrom(s => s.Name))
            .ForCtorParam("lastName", o => o.MapFrom(s => s.LastName))
            .ForCtorParam("birthDate", o => o.MapFrom(s => s.BirthDate))
            .ForMember(d => d.Email,
                c => c.MapFrom(s => s.Email))
            .AfterMap((_, person) =>
            {
                person.BirthDate = person.BirthDate.ToUniversalTime();
            });
    }
}