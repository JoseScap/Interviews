using Hexagonal.Core.Application.Ports.Driven;
using Hexagonal.Core.Domain.Entities;
using Hexagonal.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.Infrastructure.Persistence.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly HexagonalContext _context;

    public PersonRepository(HexagonalContext context)
    {
        _context = context;
    }

    public async Task<Person> AddAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public Task<List<Person>> GetAllAsync()
        => _context.People.ToListAsync();

    public Task<Person?> GetByIdAsync(int id)
        => _context.People.FirstOrDefaultAsync(x => x.Id == id);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
