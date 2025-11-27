using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.Ports.Driving;

public interface ICreateCatalogImageUseCase
{
    Task<BaseCatalogImageResponse> ExecuteAsync(CreateCatalogImageRequest request, CancellationToken cancellationToken = default);
}

public interface IListAllCatalogImagesUseCase
{
    Task<List<BaseCatalogImageResponse>> ExecuteAsync();
}

public interface IListCatalogImageByIdUseCase
{
    Task<BaseCatalogImageResponse> ExecuteAsync(Guid id);
}

public interface IDeleteCatalogImageUseCase
{
    Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default);
}