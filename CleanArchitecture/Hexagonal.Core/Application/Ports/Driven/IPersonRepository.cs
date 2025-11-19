using Hexagonal.Core.Domain.Entities;

namespace Hexagonal.Core.Application.Ports.Driven;

public interface IPersonRepository
{
    Task<Person> AddAsync(Person person);
    Task<Person?> GetByIdAsync(int id);
    Task<List<Person>> GetAllAsync();
    Task SaveChangesAsync();
}

