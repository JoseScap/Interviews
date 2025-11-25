using Core.Application.Ports.Driving;
using Core.Domain.Requests;
using Core.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

[ApiController()]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ICreateProductUseCase _createProductUseCase;
    private readonly IListAllProductsUseCase _listAllProductsUseCase;
    private readonly IListProductByIdUseCase _listProductByIdUseCase;
    private readonly IUpdateProductUseCase _updateProductUseCase;
    private readonly IDeleteProductUseCase _deleteProductUseCase;

    public ProductsController(
        ICreateProductUseCase createProduct,
        IListAllProductsUseCase listAllProducts,
        IListProductByIdUseCase listProductById,
        IUpdateProductUseCase updateProduct,
        IDeleteProductUseCase deleteProduct)
    {
        _createProductUseCase = createProduct;
        _listAllProductsUseCase = listAllProducts;
        _listProductByIdUseCase = listProductById;
        _updateProductUseCase = updateProduct;
        _deleteProductUseCase = deleteProduct;
    }

    [HttpPost]
    public async Task<ActionResult<BaseProductResponse>> Create([FromBody] CreateProductRequest request)
    {
        var result = await _createProductUseCase.ExecuteAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<BaseProductResponse>>> ListAll()
    {
        var result = await _listAllProductsUseCase.ExecuteAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseProductResponse>> ListById(Guid id)
    {
        var result = await _listProductByIdUseCase.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BaseProductResponse>> Update(
        Guid id,
        [FromBody] UpdateProductRequest request)
    {
        var result = await _updateProductUseCase.ExecuteAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        await _deleteProductUseCase.ExecuteAsync(id);
        return Ok(true);
    }
}
