using Core.Application.Ports.Driven;
using Core.Domain.Entities;
using Infrastructure.Persistence.Db;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Persistence.Repository;

public class CatalogImageRepository : ICatalogImageRepository
{
    private readonly Container _container;

    public CatalogImageRepository(DbContext dbContext)
    {
        _container = dbContext.CatalogImageContainer;
    }

    public async Task<CatalogImage> SaveAsync(CatalogImage entity)
    {
        var response = await _container.UpsertItemAsync(
            item: entity,
            partitionKey: new(entity.ProductCategory)
        );
        return response.Resource;
    }

    public async Task<List<CatalogImage>> ListAllAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = _container.GetItemQueryIterator<CatalogImage>(query);
        
        var results = new List<CatalogImage>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task<CatalogImage?> ListByIdAsync(Guid id)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
            .WithParameter("@id", id);
        var iterator = _container.GetItemQueryIterator<CatalogImage>(query);
        
        CatalogImage? result = null;

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            result = response.FirstOrDefault();
        }

        return result;
    }

    public async Task<CatalogImage?> ListByIdAndPartitionKeyAsync(Guid id, string category)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id AND c.category = @category")
            .WithParameter("@id", id)
            .WithParameter("@category", category);
        var iterator = _container.GetItemQueryIterator<CatalogImage>(query);
        
        CatalogImage? result = null;

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            result = response.FirstOrDefault();
        }

        return result;
    }

    public async Task<CatalogImage> UpdateAsync(CatalogImage entity)
    {
        var response = await _container.UpsertItemAsync(
            item: entity,
            partitionKey: new(entity.ProductCategory)
        );
        return response.Resource;
    }

    public async Task DeleteAsync(CatalogImage entity)
    {
        await _container.DeleteItemAsync<CatalogImage>(
            id: entity.Id,
            partitionKey: new(entity.ProductCategory)
        );
    }
}
