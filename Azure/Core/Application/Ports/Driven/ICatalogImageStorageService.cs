using Core.Domain.Responses;

namespace Core.Application.Ports.Driven;

public interface ICatalogImageStorageService
{
    Task<(string blobName, string blobUrl)> UploadAsync(
        Stream content,
        string blobName,
        string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string blobName,
        CancellationToken cancellationToken = default);
}