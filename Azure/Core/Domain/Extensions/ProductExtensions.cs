using Core.Domain.Entities;
using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Domain.Extensions;

public static class ProductExtensions
{
    public static Product MapToEntity(this CreateProductRequest request)
        => new(request.Category, request.Name, request.Price, request.Description);

    public static BaseProductResponse MapToBaseResponse(this Product entity)
        => new(entity.Id, entity.Category, entity.Name, entity.Price, entity.Description);

    public static (bool shouldUpdate, bool partitionKeyChanged, string previousPartitionKey) MergeWithUpdateRequest(this Product entity, UpdateProductRequest request)
    {
        var shouldUpdate = false;
        var partitionKeyWillChange = false;
        var previousPartitionKey = entity.Category;
        
        if (!string.IsNullOrEmpty(request.Name))
        {
            entity.Name = request.Name;
            shouldUpdate = true;
        }
        if (request.Price.HasValue)
        {
            entity.Price = request.Price.Value;
            shouldUpdate = true;
        }
        if (!string.IsNullOrEmpty(request.Description))
        {
            entity.Description = request.Description;
            shouldUpdate = true;
        }
        if (!string.IsNullOrEmpty(request.Category))
        {
            entity.Category = request.Category;
            shouldUpdate = true;
            partitionKeyWillChange = true;
        }

        return (shouldUpdate, partitionKeyWillChange, previousPartitionKey);
    }
}
