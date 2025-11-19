using Microsoft.EntityFrameworkCore;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Infrastructure.Database;

namespace Onion.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly OnionContext _context;

    public PersonRepository(OnionContext db)
    {
        _context = db;
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
