using Ambev.DeveloperEvaluation.Application.Commands.Sales;

namespace Ambev.DeveloperEvaluation.Application.Interfaces;

public interface ISaleService
{
    Task<SaleResponse> CreateSaleAsync(CreateSaleCommand command);
    Task<SaleResponse?> GetSaleByIdAsync(Guid id);
    Task<IEnumerable<SaleResponse>> GetAllSalesAsync();
    Task CancelSaleAsync(Guid id);
    Task RemoveItemFromSaleAsync(Guid saleId, Guid itemId);
}
