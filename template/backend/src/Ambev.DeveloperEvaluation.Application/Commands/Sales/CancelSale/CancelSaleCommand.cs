using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSale;

public record CancelSaleCommand(Guid SaleId) : IRequest;