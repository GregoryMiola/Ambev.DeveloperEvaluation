using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Mocks.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private static readonly List<ProductDto> _products = new()
    {
        new ProductDto(Guid.Parse("e4d2b0a0-4b8a-4f2a-8c9d-0e1f2a3b4c5d"), "Cerveja Edição Limitada GTH", 15.99m),
        new ProductDto(Guid.Parse("a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"), "Kit de Copos Especiais", 89.50m),
        new ProductDto(Guid.Parse("f7a8b9c0-d1e2-f3a4-b5c6-d7e8f9a0b1c2"), "Abridor de Garrafas Omnia", 25.00m),
        new ProductDto(Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "Boné Exclusivo", 45.50m),
        new ProductDto(Guid.Parse("6d5c4b3a-2f1e-0d9c-8b7a-6f5e4d3c2b1a"), "Caneca de Chopp Congelável", 62.00m)
    };

    public Task<ProductDto?> FindByIdAsync(Guid productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        return Task.FromResult(product);
    }

    public Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return Task.FromResult(_products.AsEnumerable());
    }
}
