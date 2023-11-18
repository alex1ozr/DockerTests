using AutoMapper;
using DockerTestsSample.Api.Contracts.People;
using DockerTestsSample.Api.Infrastructure.Mapping;
using DockerTestsSample.Services.People;
using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Controllers;

[ApiController]
[Route("v1/people/")]
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

    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [HttpPost("{id:guid}", Name = "CreatePerson")]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] PersonRequest request)
    {
        var personDto = request.ToPersonDto(id);

        await _personService.CreateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);

        return CreatedAtAction("Get", new { response.Id }, response);
    }

    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}", Name = "GetPerson")]
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

    [ProducesResponseType(typeof(IReadOnlyCollection<PersonResponse>), StatusCodes.Status200OK)]
    [HttpGet(Name = "GetAllPeople")]
    public async Task<IActionResult> GetAll()
    {
        var people = await _personService.GetAllAsync();
        var peopleResponse = _mapper.Map<IReadOnlyCollection<PersonResponse>>(people);
        return Ok(peopleResponse);
    }

    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpPut("{id:guid}", Name = "UpdatePerson")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PersonRequest request)
    {
        var personDto = request.ToPersonDto(id);
        await _personService.UpdateAsync(personDto);

        var response = _mapper.Map<PersonResponse>(personDto);
        return Ok(response);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:guid}", Name = "DeletePerson")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _personService.DeleteAsync(id);

        return Ok();
    }
}
