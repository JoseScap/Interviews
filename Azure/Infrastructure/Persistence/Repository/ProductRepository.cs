using Core.Application.Ports.Driven;
using Core.Domain.Entities;
using Infrastructure.Persistence.Db;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Persistence.Repository;

public class ProductRepository : IProductRepository
{
    private readonly DbContext _dbContext;

    public ProductRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> SaveAsync(Product entity)
    {
        var response = await _dbContext.ProductContainer.UpsertItemAsync(
            item: entity,
            partitionKey: new(entity.Category)
        );
        return response.Resource;
    }

    public async Task<List<Product>> ListAllAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");
        var iterator = _dbContext.ProductContainer.GetItemQueryIterator<Product>(query);
        
        var results = new List<Product>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }
        
        return results;
    }

    public async Task<Product?> ListByIdAsync(Guid id)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
            .WithParameter("@id", id);
        var iterator = _dbContext.ProductContainer.GetItemQueryIterator<Product>(query);
        
        Product? result = null;

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            result = response.FirstOrDefault();
        }

        return result;
    }

    public async Task<Product?> ListByIdAndPartitionKeyAsync(Guid id, string category)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id AND c.category = @category")
            .WithParameter("@id", id)
            .WithParameter("@category", category);
        var iterator = _dbContext.ProductContainer.GetItemQueryIterator<Product>(query);
        
        Product? result = null;

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            result = response.FirstOrDefault();
        }

        return result;
    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        var response = await _dbContext.ProductContainer.UpsertItemAsync(
            item: entity,
            partitionKey: new(entity.Category)
        );
        return response.Resource;
    }

    public async Task DeleteAsync(Product entity)
    {
        await _dbContext.ProductContainer.DeleteItemAsync<Product>(
            id: entity.Id,
            partitionKey: new(entity.Category)
        );
    }
}
