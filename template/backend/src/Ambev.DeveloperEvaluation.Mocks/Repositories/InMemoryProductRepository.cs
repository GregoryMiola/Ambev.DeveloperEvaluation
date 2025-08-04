using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Mocks.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private static readonly List<ProductDto> _products = new()
    {
        new ProductDto
        {
            Id = Guid.Parse("e4d2b0a0-4b8a-4f2a-8c9d-0e1f2a3b4c5d"),
            Name = "Cerveja Edição Limitada GTH",
            Price = 15.99m
        },
        new ProductDto
        {
            Id = Guid.Parse("a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"),
            Name = "Kit de Copos Especiais",
            Price = 89.50m
        },
        new ProductDto
        {
            Id = Guid.Parse("f7a8b9c0-d1e2-f3a4-b5c6-d7e8f9a0b1c2"),
            Name = "Abridor de Garrafas Omnia",
            Price = 25.00m
        },
        new ProductDto
        {
            Id = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"),
            Name = "Boné Exclusivo",
            Price = 45.50m
        },
        new ProductDto
        {
            Id = Guid.Parse("6d5c4b3a-2f1e-0d9c-8b7a-6f5e4d3c2b1a"),
            Name = "Caneca de Chopp Congelável",
            Price = 62.00m
        }
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
