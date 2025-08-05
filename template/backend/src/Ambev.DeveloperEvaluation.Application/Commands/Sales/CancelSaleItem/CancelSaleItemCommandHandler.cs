using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSaleItem;

public class CancelSaleItemCommandHandler : IRequestHandler<CancelSaleItemCommand>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _eventPublisher;

    public CancelSaleItemCommandHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale == null)
        {
            throw new DomainException($"Venda com ID {request.SaleId} não encontrada.");
        }

        // A lógica de negócio é delegada para a entidade de domínio
        sale.RemoveItem(request.ItemId);
        await _saleRepository.UpdateAsync(sale);

        await _eventPublisher.PublishAsync("ItemCancelled", new { request.SaleId, request.ItemId });
    }
}
