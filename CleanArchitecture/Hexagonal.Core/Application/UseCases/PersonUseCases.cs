using Hexagonal.Core.Domain.Entities;
using Hexagonal.Core.Application.Ports.Driven;
using Hexagonal.Core.Application.Ports.Driving;
using Hexagonal.Core.Domain.Requests;
using Hexagonal.Core.Domain.Responses;

namespace Hexagonal.Core.Application.UseCases;

public class CreatePersonUseCase : ICreatePersonUseCase
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonUseCase(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<BasePersonResponse> ExecuteAsync(CreatePersonRequest request)
    {
        var person = new Person(request.FirstName, request.LastName);
        await _personRepository.AddAsync(person);
        return new BasePersonResponse(person.Id, person.FirstName, person.LastName);
    }
}

public class GetPersonByIdUseCase : IGetPersonByIdUseCase
{
    private readonly IPersonRepository _personRepository;

    public GetPersonByIdUseCase(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<BasePersonResponse?> ExecuteAsync(int id)
    {
        var person = await _personRepository.GetByIdAsync(id);
        if (person == null) return null;
        return new BasePersonResponse(person.Id, person.FirstName, person.LastName);
    }
}

public class GetAllPersonsUseCase : IGetAllPersonsUseCase
{
    private readonly IPersonRepository _personRepository;

    public GetAllPersonsUseCase(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<List<BasePersonResponse>> ExecuteAsync()
    {
        var people = await _personRepository.GetAllAsync();
        return people.Select(p => new BasePersonResponse(p.Id, p.FirstName, p.LastName)).ToList();
    }
}