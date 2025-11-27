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
    private readonly ICatalogImageStorageService _catalogImageStorageService;

    public CreateCatalogImageUseCase(ICatalogImageRepository catalogImageRepository, ICatalogImageStorageService catalogImageStorageService)
    {
        _catalogImageRepository = catalogImageRepository;
        _catalogImageStorageService = catalogImageStorageService;
    }

    public async Task<BaseCatalogImageResponse> ExecuteAsync(
        CreateCatalogImageRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.File == null || request.File.Length == 0)
        {
            throw new ArgumentException("File cannot be empty.", nameof(request.File));
        }

        await using var stream = request.File.OpenReadStream();
        var id = Guid.NewGuid();
        var blobName = $"{id}-{request.File.FileName}";
        var contentType = string.IsNullOrWhiteSpace(request.File.ContentType)
            ? "application/octet-stream"
            : request.File.ContentType;

        var (uploadedBlobName, uploadedBlobUrl) = await _catalogImageStorageService.UploadAsync(
            stream,
            blobName,
            contentType,
            cancellationToken);

        var entity = new CatalogImage(id.ToString(), uploadedBlobName, uploadedBlobUrl, request.ProductCategory);
        var savedEntity = await _catalogImageRepository.SaveAsync(entity);
        return savedEntity.MapToBaseResponse();
    }
}

public class ListAllCatalogImagesUseCase: IListAllCatalogImagesUseCase
{
    private readonly ICatalogImageRepository _catalogImageRepository;

    public ListAllCatalogImagesUseCase(ICatalogImageRepository catalogImageRepository)
    {
        _catalogImageRepository = catalogImageRepository;
    }

    public async Task<List<BaseCatalogImageResponse>> ExecuteAsync()
    {
        var entities = await _catalogImageRepository.ListAllAsync();
        return entities.Select(e => e.MapToBaseResponse()).ToList();
    }
}

public class ListCatalogImageByIdUseCase: IListCatalogImageByIdUseCase
{
    private readonly ICatalogImageRepository _catalogImageRepository;

    public ListCatalogImageByIdUseCase(ICatalogImageRepository catalogImageRepository)
    {
        _catalogImageRepository = catalogImageRepository;
    }

    public async Task<BaseCatalogImageResponse> ExecuteAsync(Guid id)
    {
        var entity = await _catalogImageRepository.ListByIdAsync(id);
        if (entity == null)
        {
            throw new Exception("Entity with id {id} not found");
        }
        return entity.MapToBaseResponse();
    }
}

public class DeleteCatalogImageUseCase: IDeleteCatalogImageUseCase
{
    private readonly ICatalogImageRepository _catalogImageRepository;
    private readonly ICatalogImageStorageService _catalogImageStorageService;

    public DeleteCatalogImageUseCase(ICatalogImageRepository catalogImageRepository, ICatalogImageStorageService catalogImageStorageService)
    {
        _catalogImageRepository = catalogImageRepository;
        _catalogImageStorageService = catalogImageStorageService;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _catalogImageRepository.ListByIdAsync(id);
        if (entity == null)
        {
            throw new Exception("Entity with id {id} not found");
        }
        await _catalogImageStorageService.DeleteAsync(entity.BlobName, cancellationToken);
        await _catalogImageRepository.DeleteAsync(entity);
    }
}