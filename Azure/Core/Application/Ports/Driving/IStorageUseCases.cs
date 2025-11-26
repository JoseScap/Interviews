using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.Ports.Driving;

public interface IUploadCatalogBlobUseCase
{
    Task<BaseCatalogResponse> ExecuteAsync(
        UploadCatalogImageRequest request,
        long maxAllowedSizeInBytes,
        string containerName,
        CancellationToken cancellationToken = default);
}

public interface IGetAllCatalogBlobsUseCase
{
    Task<List<BaseCatalogResponse>> ExecuteAsync(
        string containerName,
        CancellationToken cancellationToken = default);
}

public interface IDeleteCatalogBlobUseCase
{
    Task ExecuteAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);
}