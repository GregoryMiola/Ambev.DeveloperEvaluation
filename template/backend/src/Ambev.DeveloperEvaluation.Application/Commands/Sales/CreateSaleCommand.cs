using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

public class CreateSaleCommand
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; } // Assumindo que BranchId também é um Guid
    public List<SaleItemCommand> Items { get; set; } = new();
}