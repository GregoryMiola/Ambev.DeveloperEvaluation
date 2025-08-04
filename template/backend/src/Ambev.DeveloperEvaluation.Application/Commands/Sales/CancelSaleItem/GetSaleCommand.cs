using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSaleItem;

public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest;