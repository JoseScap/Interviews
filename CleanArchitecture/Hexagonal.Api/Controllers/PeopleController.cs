using Hexagonal.Core.Application.Ports.Driving;
using Hexagonal.Core.Domain.Responses;
using Hexagonal.Core.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Onion.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly ICreatePersonUseCase _createPersonUseCase;
    private readonly IGetPersonByIdUseCase _getPersonByIdUseCase;
    private readonly IGetAllPersonsUseCase _getAllPersonsUseCase;

    public PeopleController(
        ICreatePersonUseCase createPersonUseCase,
        IGetPersonByIdUseCase getPersonByIdUseCase,
        IGetAllPersonsUseCase getAllPersonsUseCase)
    {
        _createPersonUseCase = createPersonUseCase;
        _getPersonByIdUseCase = getPersonByIdUseCase;
        _getAllPersonsUseCase = getAllPersonsUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<BasePersonResponse>> Create(CreatePersonRequest request)
    {
        var result = await _createPersonUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BasePersonResponse>> GetById(int id)
    {
        var person = await _getPersonByIdUseCase.ExecuteAsync(id);
        if (person == null) return NotFound();
        return Ok(person);
    }

    [HttpGet]
    public async Task<ActionResult<List<BasePersonResponse>>> GetAll()
    {
        var people = await _getAllPersonsUseCase.ExecuteAsync();
        return Ok(people);
    }
}
