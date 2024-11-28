using AutoMapper;
using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Services.People;

namespace DockerTestsSample.Api.Infrastructure.Mapping;

internal sealed class ApiContractToDtoMappingProfile : Profile
{
    public ApiContractToDtoMappingProfile()
    {
        CreateMap<PersonRequest, PersonDto>()
            .ForMember(d => d.Id, c => c.Ignore());
    }
}