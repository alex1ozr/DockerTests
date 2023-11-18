using AutoMapper;
using DockerTestsSample.Common.Exceptions;
using DockerTestsSample.Repositories.People;
using DockerTestsSample.Services.Dto;
using DockerTestsSample.Store.Entities;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DockerTestsSample.Services.People;

[UsedImplicitly]
internal sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonService> _logger;

    public PersonService(
        IPersonRepository personRepository,
        IMapper mapper,
        ILogger<PersonService> logger)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _logger = logger;
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
        
        _logger.LogInformation("Person with Id {PersonId} was created", personDto.Id);
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
        
        _logger.LogInformation("Person with Id {PersonId} was updated", personDto.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        if (await _personRepository.GetAsync(id, ct) == null)
        {
            throw new PersonNotFoundException(id);
        }

        await _personRepository.DeleteAsync(id, ct);
        
        _logger.LogInformation("Person with Id {PersonId} was deleted", id);
    }
}
