using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.Application.Interfaces;

public interface IProductRepository
{
    Task<ProductDto?> FindByIdAsync(Guid productId);
    Task<IEnumerable<ProductDto>> GetAllAsync();
}
