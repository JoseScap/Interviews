using Microsoft.AspNetCore.Mvc;
using Onion.Application.Interfaces;
using Onion.Domain.Requests;
using Onion.Domain.Responses;

namespace Onion.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _service;

    public PeopleController(IPersonService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<BasePersonResponse>> Create(CreatePersonRequest request)
    {
        var person = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(ListById), new { person.Id }, person);
    }

    [HttpGet]
    public async Task<ActionResult<List<BasePersonResponse>>> ListAll()
    {
        var peopleList = await _service.ListAllAsync();
        return Ok(peopleList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BasePersonResponse>> ListById(int id)
    {
        var person = await _service.ListOneByIdAsync(id);
        if (person == null) return NotFound();
        return Ok(person);
    }
}
