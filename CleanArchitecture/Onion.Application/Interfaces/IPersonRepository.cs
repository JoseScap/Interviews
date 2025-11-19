using Onion.Domain.Entities;

namespace Onion.Application.Interfaces;

public interface IPersonRepository
{
    Task<Person> AddAsync(Person person);
    Task<Person?> GetByIdAsync(int id);
    Task<List<Person>> GetAllAsync();
    Task SaveChangesAsync();
}
