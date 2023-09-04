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
        CreateMap<PersonRequest, PersonDto>()
            .ForMember(d => d.Id, c => c.Ignore());

        CreateMap<UpdatePersonRequest, PersonDto>()
            .ForMember(d=> d.Id, c => c.MapFrom(s=> s.Id))
            .ForMember(d => d.Name,
                c => c.MapFrom(s => s.Person!.Name))
            .ForMember(d => d.LastName,
                c => c.MapFrom(s => s.Person!.LastName))
            .ForMember(d => d.BirthDate,
                c => c.MapFrom(s => s.Person!.BirthDate))
            .ForMember(d => d.Email,
                c => c.MapFrom(s => s.Person!.Email));
    }
}