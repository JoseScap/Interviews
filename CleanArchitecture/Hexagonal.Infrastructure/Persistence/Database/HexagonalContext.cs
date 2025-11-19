using Hexagonal.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.Infrastructure.Persistence.Database;

public class HexagonalContext: DbContext
{
    public DbSet<Person> People { get; set; }

    public HexagonalContext(DbContextOptions<HexagonalContext> options)
        : base(options)
    {
    }
}
