using System;
using Core.Application.Ports.Driving;
using Core.Domain.Requests;
using Core.Domain.Responses;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IUploadCatalogBlobUseCase _uploadCatalogBlobUseCase;
    private readonly IGetAllCatalogBlobsUseCase _getAllCatalogBlobsUseCase;
    private readonly IDeleteCatalogBlobUseCase _deleteCatalogBlobUseCase;
    private readonly StorageContext _storageContext;

    public CatalogController(
        IUploadCatalogBlobUseCase uploadCatalogBlobUseCase,
        IGetAllCatalogBlobsUseCase getAllCatalogBlobsUseCase,
        IDeleteCatalogBlobUseCase deleteCatalogBlobUseCase,
        StorageContext storageContext)
    {
        _uploadCatalogBlobUseCase = uploadCatalogBlobUseCase;
        _getAllCatalogBlobsUseCase = getAllCatalogBlobsUseCase;
        _deleteCatalogBlobUseCase = deleteCatalogBlobUseCase;
        _storageContext = storageContext;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<BaseCatalogResponse>> UploadImage(
        [FromForm] UploadCatalogImageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _uploadCatalogBlobUseCase.ExecuteAsync(
            request,
            _storageContext.CatalogMaxSizeInBytes,
            _storageContext.Catalog.Name,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<BaseCatalogResponse>>> GetAllBlobs(CancellationToken cancellationToken)
    {
        var response = await _getAllCatalogBlobsUseCase.ExecuteAsync(
            _storageContext.Catalog.Name,
            cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> DeleteBlob(
        string blobName,
        CancellationToken cancellationToken)
    {
        await _deleteCatalogBlobUseCase.ExecuteAsync(
            _storageContext.Catalog.Name,
            blobName,
            cancellationToken);
        return NoContent();
    }
}

