using Core.Application.Ports.Driven;
using Core.Application.Ports.Driving;
using Core.Domain.Entities;
using Core.Domain.Extensions;
using Core.Domain.Requests;
using Core.Domain.Responses;

namespace Core.Application.UseCases;

public class CreateCatalogImageUseCase: ICreateCatalogImageUseCase
{
    private readonly ICatalogImageRepository _catalogImageRepository;
    private readonly IStorageService _storageService;

    public CreateCatalogImageUseCase(ICatalogImageRepository catalogImageRepository, IStorageService storageService)
    {
        _catalogImageRepository = catalogImageRepository;
        _storageService = storageService;
    }

    public async Task<BaseCatalogImageResponse> ExecuteAsync(
        CreateCatalogImageRequest request,
        long maxAllowedSizeInBytes,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("File cannot be empty.", nameof(request.File));
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

        var (uploadedBlobName, uploadedBlobUrl) = await _storageService.UploadAsync(
            containerName,
            stream,
            blobName,
            contentType,
            cancellationToken);

        var entity = new CatalogImage(uploadedBlobName, uploadedBlobUrl, request.ProductCategory);
        var savedEntity = await _catalogImageRepository.SaveAsync(entity);
        return savedEntity.MapToBaseResponse();
    }
}