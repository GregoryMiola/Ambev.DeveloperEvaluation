using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSales;

public record GetSalesCommand : IRequest<IEnumerable<SaleResponse>>;