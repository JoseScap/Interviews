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
    private readonly IListAllCatalogImagesUseCase _listAllCatalogImagesUseCase;
    private readonly IListCatalogImageByIdUseCase _listCatalogImageByIdUseCase;
    private readonly IDeleteCatalogImageUseCase _deleteCatalogImageUseCase;

    public CatalogImageController(
        ICreateCatalogImageUseCase createCatalogImageUseCase,
        IListAllCatalogImagesUseCase listAllCatalogImagesUseCase,
        IListCatalogImageByIdUseCase listCatalogImageByIdUseCase,
        IDeleteCatalogImageUseCase deleteCatalogImageUseCase)
    {
        _createCatalogImageUseCase = createCatalogImageUseCase;
        _listAllCatalogImagesUseCase = listAllCatalogImagesUseCase;
        _listCatalogImageByIdUseCase = listCatalogImageByIdUseCase;
        _deleteCatalogImageUseCase = deleteCatalogImageUseCase;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<BaseCatalogImageResponse>> Create(
        [FromForm] CreateCatalogImageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _createCatalogImageUseCase.ExecuteAsync(
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<BaseCatalogImageResponse>>> ListAll()
    {
        var response = await _listAllCatalogImagesUseCase.ExecuteAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseCatalogImageResponse>> ListById(Guid id)
    {
        var response = await _listCatalogImageByIdUseCase.ExecuteAsync(id);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _deleteCatalogImageUseCase.ExecuteAsync(id, cancellationToken);
        return Ok();
    }
}

