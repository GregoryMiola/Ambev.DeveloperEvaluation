using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

public record SaleResponse
{
    public Guid Id { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public decimal TotalAmount { get; init; }
    public bool IsCancelled { get; init; }
    public List<SaleItemResponse> Items { get; init; } = new();
}