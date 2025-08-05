using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Common.Models;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Queries.Sales.GetSales;

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, PaginatedList<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesQueryHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SaleResponse>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var paginatedSales = await _saleRepository.GetAllPaginatedAsync(request.PageNumber, request.PageSize);
        var mappedItems = _mapper.Map<List<SaleResponse>>(paginatedSales);

        return new PaginatedList<SaleResponse>(mappedItems, paginatedSales.TotalCount, paginatedSales.CurrentPage, paginatedSales.PageSize);
    }
}
