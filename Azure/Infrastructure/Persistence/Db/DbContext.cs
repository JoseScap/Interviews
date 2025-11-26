using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Infrastructure.Security;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Persistence.Db;

public class DbContext
{
    public CosmosClient Client;
    public Database Database { get; private set; } = null!;
    public Container ProductContainer { get; private set; } = null!;

    public DbContext(KeyVaultContext keyVault)
    {
        var cosmosSecret = new CosmosSecret(keyVault.CosmosSecret.Value);
        Client = new(connectionString: cosmosSecret.Value);
    }

    public async Task InitializeAsync()
    {
        Database = await Client.CreateDatabaseIfNotExistsAsync("Database");

        var productContainerProps = new ContainerProperties("Product", Product.PartitionKeyPath);
        ProductContainer = await Database.CreateContainerIfNotExistsAsync(productContainerProps);
    }
}