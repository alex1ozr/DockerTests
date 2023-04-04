using AutoMapper;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Api.Infrastructure.Attributes;
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

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("{id:guid}")]
    public async Task<ActionResult<PersonResponse>> Create([FromMultiSource] CreatePersonRequest request)
    {
        var personDto = _mapper.Map<PersonDto>(request);

        await _personService.CreateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);

        return CreatedAtAction("Get", new { response.Id }, response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PersonResponse>> Get([FromRoute] Guid id)
    {
        var personDto = await _personService.GetAsync(id);

        if (personDto is null)
        {
            return NotFound();
        }

        var personResponse = _mapper.Map<PersonResponse>(personDto);
        return Ok(personResponse);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PersonResponse>>> GetAll()
    {
        var people = await _personService.GetAllAsync();
        var peopleResponse = _mapper.Map<IReadOnlyCollection<PersonResponse>>(people);
        return Ok(peopleResponse);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PersonResponse>> Update([FromMultiSource] UpdatePersonRequest request)
    {
        var personDto = _mapper.Map<PersonDto>(request);
        await _personService.UpdateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _personService.DeleteAsync(id);

        return Ok();
    }
}
