using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Common.Models;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Sales.GetSales;

public record GetSalesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<SaleResponse>>;