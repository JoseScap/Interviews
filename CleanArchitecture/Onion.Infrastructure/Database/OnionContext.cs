using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;

namespace Onion.Infrastructure.Database;

public class OnionContext: DbContext
{
    public DbSet<Person> People { get; set; }

    public OnionContext(DbContextOptions<OnionContext> options)
        : base(options)
    {
    }
}
