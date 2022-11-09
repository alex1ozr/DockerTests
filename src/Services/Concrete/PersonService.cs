using AutoMapper;
using DockerTestsSample.Common.Exceptions;
using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.Repositories.Abstract;
using DockerTestsSample.Services.Abstract;
using DockerTestsSample.Services.Dto;
using JetBrains.Annotations;

namespace DockerTestsSample.Services.Concrete;

[UsedImplicitly]
internal sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public PersonService(
        IPersonRepository personRepository,
        IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(PersonDto personDto, CancellationToken ct)
    {
        var entity = await _personRepository.GetAsync(personDto.Id, ct);
        if (entity is not null)
        {
            throw new PersonAlreadyExistsException(personDto.Id);
        }

        entity = _mapper.Map<Person>(personDto);
        await _personRepository.CreateAsync(entity, ct);
    }

    public async Task<PersonDto?> GetAsync(Guid id, CancellationToken ct)
    {
        var entity = await _personRepository.GetAsync(id, ct);
        return _mapper.Map<PersonDto>(entity);
    }

    public async Task<IReadOnlyCollection<PersonDto>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _personRepository.GetAllAsync(ct);
        return _mapper.Map<IReadOnlyCollection<PersonDto>>(entities);
    }

    public async Task UpdateAsync(PersonDto personDto, CancellationToken ct)
    {
        var entity = await _personRepository.GetAsync(personDto.Id, ct)
                     ?? throw new PersonNotFoundException(personDto.Id);

        _mapper.Map(personDto, entity);
        await _personRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (await _personRepository.GetAsync(id, ct) == null)
        {
            throw new PersonNotFoundException(id);
        }

        await _personRepository.DeleteAsync(id, ct);
    }
}
