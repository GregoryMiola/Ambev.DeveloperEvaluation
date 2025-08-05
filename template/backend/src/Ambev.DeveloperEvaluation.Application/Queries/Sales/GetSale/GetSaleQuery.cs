using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Sales.GetSale;
 
public record GetSaleQuery (Guid SaleId) : IRequest<SaleResponse?>;