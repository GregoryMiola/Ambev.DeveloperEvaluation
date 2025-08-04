using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSale;

public record GetSaleCommand (Guid SaleId) : IRequest<SaleResponse>;