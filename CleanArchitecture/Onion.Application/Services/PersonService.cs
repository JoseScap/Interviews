using Onion.Application.Interfaces;
using Onion.Domain.Extensions;
using Onion.Domain.Requests;
using Onion.Domain.Responses;

namespace Onion.Application.Services;

public class PersonService: IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async  Task<BasePersonResponse> CreateAsync(CreatePersonRequest request)
    {
        var entity = request.MapToEntity();
        await _personRepository.AddAsync(entity);
        return entity.MapToBaseResponse();
    }

    public async Task<List<BasePersonResponse>> ListAllAsync()
    {
        var entityList = await _personRepository.GetAllAsync();
        return entityList.Select(p => p.MapToBaseResponse()).ToList();
    }

    public async Task<BasePersonResponse?> ListOneByIdAsync(int id)
    {
        var entity = await _personRepository.GetByIdAsync(id);

        if (entity == null)
        {
            return null;
        }

        return entity.MapToBaseResponse();
    }
}
