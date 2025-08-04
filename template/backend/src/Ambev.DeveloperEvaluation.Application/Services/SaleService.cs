using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class SaleService : ISaleService
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;

    public SaleService(IUserRepository userRepository, IProductRepository productRepository, ISaleRepository saleRepository)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _saleRepository = saleRepository;
    }

    public async Task<SaleResponse> CreateSaleAsync(CreateSaleCommand command)
    {
        // 1. Validate external identities
        var userDto = await _userRepository.FindByIdAsync(command.CustomerId);
        if (userDto == null)
        {
            throw new DomainException($"Cliente com ID {command.CustomerId} não encontrado.");
        }

        // 2. Create the aggregate root
        var sale = Sale.Create(command.CustomerId, userDto.Name, command.BranchId);

        // 3. Process each item
        foreach (var itemCommand in command.Items)
        {
            var productDto = await _productRepository.FindByIdAsync(itemCommand.ProductId);
            if (productDto == null)
            {
                throw new DomainException($"Produto com ID {itemCommand.ProductId} não encontrado.");
            }

            // 4. Map DTO to a transient Domain Entity to pass to the aggregate
            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price
            };

            // 5. Add item to the aggregate, which enforces its own business rules
            sale.AddItem(product, itemCommand.Quantity);
        }

        // 6. Persist the entire aggregate atomically
        await _saleRepository.AddAsync(sale);

        // 7. Map the final aggregate to a response DTO (implementation omitted for brevity)
        return SaleResponse.FromSale(sale);
    }

    public async Task<SaleResponse?> GetSaleByIdAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
        {
            return null;
        }

        return SaleResponse.FromSale(sale);
    }
    
    public async Task<IEnumerable<SaleResponse>> GetAllSalesAsync()
    {
        var sales = await _saleRepository.GetAllAsync();
        return sales.Select(SaleResponse.FromSale).ToList();
    }

    public async Task CancelSaleAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
        {
            throw new DomainException($"Venda com ID {id} não encontrada.");
        }

        sale.Cancel();
        await _saleRepository.UpdateAsync(sale);
    }

    public async Task RemoveItemFromSaleAsync(Guid saleId, Guid itemId)
    {
        var sale = await _saleRepository.GetByIdAsync(saleId);
        if (sale == null)
        {
            throw new DomainException($"Venda com ID {saleId} não encontrada.");
        }

        // A lógica de negócio é delegada para a entidade de domínio
        sale.RemoveItem(itemId);
        await _saleRepository.UpdateAsync(sale);
    }
}
