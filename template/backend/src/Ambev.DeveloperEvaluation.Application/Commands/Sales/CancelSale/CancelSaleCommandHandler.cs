using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSale;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public CancelSaleCommandHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId);
        if (sale == null)
        {
            throw new DomainException($"Venda com ID {request.SaleId} não encontrada.");
        }

        sale.Cancel();
        await _saleRepository.UpdateAsync(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync("SaleCancelled", new { request.SaleId });
    }
}
