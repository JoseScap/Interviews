using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.Ports.Driving;

public interface ICreateProductUseCase
{
    Task<BaseProductResponse> ExecuteAsync(CreateProductRequest request);
}

public interface IListAllProductsUseCase
{
    Task<List<BaseProductResponse>> ExecuteAsync();
}

public interface IListProductByIdUseCase
{
    Task<BaseProductResponse> ExecuteAsync(Guid id);
}

public interface IUpdateProductUseCase
{
    Task<BaseProductResponse> ExecuteAsync(Guid id, UpdateProductRequest request);
}

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(Guid id);
}
