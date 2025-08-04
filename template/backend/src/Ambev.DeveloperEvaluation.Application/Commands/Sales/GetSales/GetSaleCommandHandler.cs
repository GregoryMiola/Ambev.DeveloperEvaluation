using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Common.Models;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSales;

public class GetSalesCommandHandler : IRequestHandler<GetSalesCommand, PaginatedList<SaleResponse>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SaleResponse>> Handle(GetSalesCommand request, CancellationToken cancellationToken)
    {
        var paginatedSales = await _saleRepository.GetAllPaginatedAsync(request.PageNumber, request.PageSize);
        var mappedItems = _mapper.Map<List<SaleResponse>>(paginatedSales);

        return new PaginatedList<SaleResponse>(mappedItems, paginatedSales.TotalCount, paginatedSales.CurrentPage, paginatedSales.PageSize);
    }
}
