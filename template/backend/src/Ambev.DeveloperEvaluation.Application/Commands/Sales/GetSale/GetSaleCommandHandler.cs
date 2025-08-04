using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSale;

public class GetSaleCommandHandler : IRequestHandler<GetSaleCommand, SaleResponse?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<SaleResponse?> Handle(GetSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale == null)
        {
            return null;
        }

        return _mapper.Map<SaleResponse>(sale);
    }
}
