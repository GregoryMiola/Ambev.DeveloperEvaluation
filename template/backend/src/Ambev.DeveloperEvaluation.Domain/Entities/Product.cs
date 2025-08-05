namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents the concept of a Product within the Sales domain.
/// It's a transient object created from a DTO to enforce domain rules.
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
