using System;
using Core.Application.Ports.Driving;
using Core.Domain.Requests;
using Core.Domain.Responses;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogImageController : ControllerBase
{
    private readonly ICreateCatalogImageUseCase _createCatalogImageUseCase;
    private readonly StorageContext _storageContext;

    public CatalogImageController(
        ICreateCatalogImageUseCase createCatalogImageUseCase,
        StorageContext storageContext)
    {
        _createCatalogImageUseCase = createCatalogImageUseCase;
        _storageContext = storageContext;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<BaseCatalogImageResponse>> Create(
        [FromForm] CreateCatalogImageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _createCatalogImageUseCase.ExecuteAsync(
            request,
            _storageContext.CatalogMaxSizeInBytes,
            _storageContext.Catalog.Name,
            cancellationToken);

        return Ok(response);
    }
}

