namespace Ambev.DeveloperEvaluation.Application.Interfaces;

/// <summary>
/// Represents a product from the external catalog service.
/// This is a DTO, not a domain entity.
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Represents a user/customer from the external identity service.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public interface IProductRepository
{
    Task<ProductDto?> FindByIdAsync(Guid productId);
    Task<IEnumerable<ProductDto>> GetAllAsync();
}

public interface IUserRepository
{
    Task<UserDto?> FindByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetAllAsync();
}

