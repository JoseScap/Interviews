using Core.Application.Ports.Driven;
using Core.Application.Ports.Driving;
using Core.Domain.Entities;
using Core.Domain.Extensions;
using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.UseCases;

public class CreateProductUseCase: ICreateProductUseCase
{
    private readonly IProductRepository _repository;

    public CreateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseProductResponse> ExecuteAsync(CreateProductRequest request)
    {
        var entity = request.MapToEntity();
        var savedEntity = await _repository.SaveAsync(entity);
        return savedEntity.MapToBaseResponse();
    }
}

public class ListAllProductsUseCase: IListAllProductsUseCase
{
    private readonly IProductRepository _repository;

    public ListAllProductsUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BaseProductResponse>> ExecuteAsync()
    {
        var entities = await _repository.ListAllAsync();
        return entities.Select(e => e.MapToBaseResponse()).ToList();
    }
}

public class ListProductByIdUseCase: IListProductByIdUseCase
{
    private readonly IProductRepository _repository;

    public ListProductByIdUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseProductResponse> ExecuteAsync(Guid id)
    {
        var entity = await _repository.ListByIdAsync(id);
        if (entity == null)
        {
            throw new Exception("Entity with id {id} not found");
        }
        return entity.MapToBaseResponse();
    }
}

public class UpdateProductUseCase: IUpdateProductUseCase
{
    private readonly IProductRepository _repository;

    public UpdateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseProductResponse> ExecuteAsync(Guid id, UpdateProductRequest request)
    {
        var entity = await _repository.ListByIdAsync(id);

        if (entity == null)
        {
            throw new Exception("Entity with id {id} not found");
        }

        var (shouldUpdate, partitionKeyWillChange, previousPartitionKey) = entity.MergeWithUpdateRequest(request);

        if (!shouldUpdate)
        {
            return entity.MapToBaseResponse();
        }
        
        entity = await _repository.UpdateAsync(entity);

        if (partitionKeyWillChange)
        {
            var entityToDelete = new Product(entity.Id, previousPartitionKey, entity.Name, entity.Price, entity.Description);
            await _repository.DeleteAsync(entityToDelete);
        }

        return entity.MapToBaseResponse();
    }
}

public class DeleteProductUseCase: IDeleteProductUseCase
{
    private readonly IProductRepository _repository;

    public DeleteProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        var entity = await _repository.ListByIdAsync(id);
        if (entity == null)
        {
            throw new Exception("Entity with id {id} not found");
        }
        await _repository.DeleteAsync(entity);
    }
}
