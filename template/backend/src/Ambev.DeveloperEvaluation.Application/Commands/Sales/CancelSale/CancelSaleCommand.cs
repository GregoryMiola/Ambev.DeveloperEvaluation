using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSale;

public record CancelSaleCommand(Guid SaleId) : IRequest;