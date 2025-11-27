using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.Ports.Driving;

public interface ICreateCatalogImageUseCase
{
    Task<BaseCatalogImageResponse> ExecuteAsync(CreateCatalogImageRequest request, long maxAllowedSizeInBytes, string containerName, CancellationToken cancellationToken = default);
}

public interface IListAllCatalogImagesUseCase
{
    Task<List<BaseCatalogImageResponse>> ExecuteAsync();
}
