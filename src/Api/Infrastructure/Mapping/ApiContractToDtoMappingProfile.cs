using AutoMapper;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Services.Dto;
using JetBrains.Annotations;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

[UsedImplicitly]
internal sealed class ApiContractToDtoMappingProfile: Profile
{
    public ApiContractToDtoMappingProfile()
    {
        CreateMap<CreatePersonRequest, PersonDto>()
            .ForCtorParam("id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("name", o => o.MapFrom(s => s.Person.Name))
            .ForCtorParam("lastName", o => o.MapFrom(s => s.Person.LastName))
            .ForCtorParam("birthDate", o => o.MapFrom(s => s.Person.BirthDate))
            .ForMember(d => d.Email,
                c => c.MapFrom(s => s.Person.Email));

        CreateMap<UpdatePersonRequest, PersonDto>()
            .ForCtorParam("id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("name", o => o.MapFrom(s => s.Person.Name))
            .ForCtorParam("lastName", o => o.MapFrom(s => s.Person.LastName))
            .ForCtorParam("birthDate", o => o.MapFrom(s => s.Person.BirthDate))
            .ForMember(d => d.Email,
                c => c.MapFrom(s => s.Person.Email));
    }
}