using Ambev.DeveloperEvaluation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Lista todos os produtos disponíveis para venda (mockados).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAvailableProducts()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }
}
