using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;

public record CreateSaleCommand (Guid CustomerId, Guid BranchId, List<SaleItemCommand> Items) : IRequest<SaleResponse>;
