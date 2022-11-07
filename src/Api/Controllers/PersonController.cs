using AutoMapper;
using DockerTestsSample.Api.Attributes;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Services.Abstract;
using DockerTestsSample.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Controllers;

[ApiController]
[Route("people/")]
public sealed class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public PersonController(
        IPersonService personService,
        IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> Create([FromMultiSource] CreatePersonRequest request)
    {
        var personDto = _mapper.Map<PersonDto>(request);

        await _personService.CreateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);

        return CreatedAtAction("Get", new { response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var personDto = await _personService.GetAsync(id);

        if (personDto is null)
        {
            return NotFound();
        }

        var personResponse = _mapper.Map<PersonResponse>(personDto);
        return Ok(personResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var people = await _personService.GetAllAsync();
        var peopleResponse = _mapper.Map<IReadOnlyCollection<PersonResponse>>(people);
        return Ok(peopleResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromMultiSource] UpdatePersonRequest request)
    {
        var existingPerson = await _personService.GetAsync(request.Id);
        if (existingPerson is null)
        {
            return NotFound();
        }

        var personDto = _mapper.Map<PersonDto>(request);
        await _personService.UpdateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var existingPerson = await _personService.GetAsync(id);
        if (existingPerson is null)
        {
            return NotFound();
        }
        
        await _personService.DeleteAsync(id);

        return Ok();
    }
}
