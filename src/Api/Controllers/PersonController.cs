using AutoMapper;
using DockerTestsSample.Api.Attributes;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Api.Contracts.Responses;
using DockerTestsSample.Services.Abstract;
using DockerTestsSample.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DockerTestsSample.Api.Controllers;

[ApiController]
[Route("persons/")]
public sealed class PersonController : ControllerBase
{
    private readonly IPersonService _customerService;
    private readonly IMapper _mapper;

    public PersonController(
        IPersonService customerService,
        IMapper mapper)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonRequest request)
    {
        var personDto = _mapper.Map<PersonDto>(request);

        await _customerService.CreateAsync(personDto);

        var customerResponse = _mapper.Map<PersonResponse>(personDto);

        return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var personDto = await _customerService.GetAsync(id);

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
        var people = await _customerService.GetAllAsync();
        var peopleResponse = _mapper.Map<IReadOnlyCollection<PersonResponse>>(people);
        return Ok(peopleResponse);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromMultiSource] UpdatePersonRequest request)
    {
        var existingPerson = await _customerService.GetAsync(request.Id);

        if (existingPerson is null)
        {
            return NotFound();
        }

        var personDto = _mapper.Map<PersonDto>(request);
        await _customerService.UpdateAsync(personDto);

        var customerResponse = _mapper.Map<PersonResponse>(personDto);
        return Ok(customerResponse);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var existingPerson = await _customerService.GetAsync(id);
        if (existingPerson is null)
        {
            return NotFound();
        }
        
        await _customerService.DeleteAsync(id);

        return Ok();
    }
}
