using Core.Domain.Responses;

namespace Core.Application.Ports.Driven;

public interface IStorageService
{
    Task<BaseCatalogResponse> UploadAsync(
        string containerName,
        Stream content,
        string blobName,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<List<BaseCatalogResponse>> GetAllBlobsAsync(
        string containerName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);
}