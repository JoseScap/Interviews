using Core.Application.Ports.Driven;
using Core.Application.Ports.Driving;
using Core.Domain.Requests;
using Core.Domain.Responses;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.UseCases;

public class UploadCatalogBlobUseCase : IUploadCatalogBlobUseCase
{
    private readonly IStorageService _storageService;

    public UploadCatalogBlobUseCase(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<BaseCatalogResponse> ExecuteAsync(
        UploadCatalogImageRequest request,
        long maxAllowedSizeInBytes,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("File cannot be empty.", nameof(request));
        }

        if (request.File.Length > maxAllowedSizeInBytes)
        {
            var maxKb = maxAllowedSizeInBytes / 1024;
            var currentKb = request.File.Length / 1024;
            throw new ArgumentException($"File exceeds the maximum allowed size of {maxKb} KB. Current size: {currentKb} KB.");
        }

        await using var stream = request.File.OpenReadStream();
        var blobName = $"{Guid.NewGuid()}-{request.File.FileName}";
        var contentType = string.IsNullOrWhiteSpace(request.File.ContentType)
            ? "application/octet-stream"
            : request.File.ContentType;

        return await _storageService.UploadAsync(
            containerName,
            stream,
            blobName,
            contentType,
            cancellationToken);
    }
}

public class GetAllCatalogBlobsUseCase : IGetAllCatalogBlobsUseCase
{
    private readonly IStorageService _storageService;

    public GetAllCatalogBlobsUseCase(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<List<BaseCatalogResponse>> ExecuteAsync(string containerName, CancellationToken cancellationToken = default)
    {
        return await _storageService.GetAllBlobsAsync(containerName, cancellationToken);
    }
}

public class DeleteCatalogBlobUseCase : IDeleteCatalogBlobUseCase
{
    private readonly IStorageService _storageService;

    public DeleteCatalogBlobUseCase(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task ExecuteAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("Blob name is required.", nameof(blobName));
        }

        await _storageService.DeleteAsync(containerName, blobName, cancellationToken);
    }
}