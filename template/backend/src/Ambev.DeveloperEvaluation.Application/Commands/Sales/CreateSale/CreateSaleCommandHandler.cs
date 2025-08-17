using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CreateSaleCommandHandler(IUserRepository userRepository, IProductRepository productRepository, ISaleRepository saleRepository, IUnitOfWork unitOfWork, IMapper mapper, IEventPublisher eventPublisher)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<SaleResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate external identities
        var userDto = await _userRepository.FindByIdAsync(request.CustomerId);
        if (userDto == null)
        {
            throw new DomainException($"Cliente com ID {request.CustomerId} não encontrado.");
        }

        // 2. Create the aggregate root
        var sale = Sale.Create(request.CustomerId, userDto.Name, request.BranchId);

        // 3. Process each item
        foreach (var itemCommand in request.Items)
        {
            var productDto = await _productRepository.FindByIdAsync(itemCommand.ProductId) ?? throw new DomainException($"Produto com ID {itemCommand.ProductId} não encontrado.");
            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price
            };

            sale.AddItem(product, itemCommand.Quantity);
        }

        await _saleRepository.AddAsync(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var saleResponse = _mapper.Map<SaleResponse>(sale);

        await _eventPublisher.PublishAsync("SaleCreated", saleResponse);

        return saleResponse;
    }
}
