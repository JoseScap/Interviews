using Core.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Db;

public class DbContext
{
    public CosmosClient Client;
    public Database Database { get; private set; } = null!;
    public Container ProductContainer { get; private set; } = null!;

    public DbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CosmosDb");

        if (connectionString == null)
        {
            throw new ArgumentException("CosmosDb value is missing or not configured.");
        }

        Client = new(connectionString: connectionString);
    }

    public async Task InitializeAsync()
    {
        Database = await Client.CreateDatabaseIfNotExistsAsync("Database");

        var productContainerProps = new ContainerProperties("Product", Product.PartitionKeyPath);
        ProductContainer = await Database.CreateContainerIfNotExistsAsync(productContainerProps);
    }
}