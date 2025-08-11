namespace Ambev.DeveloperEvaluation.Application.Commands.SaleItems;

public record SaleItemCommand(Guid ProductId, int Quantity);
