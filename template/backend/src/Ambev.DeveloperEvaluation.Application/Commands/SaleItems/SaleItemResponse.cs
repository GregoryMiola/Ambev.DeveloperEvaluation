using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Commands.SaleItems;

public class SaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; } = false;

    public static SaleItemResponse FromSaleItem(SaleItem item)
    {
        return new SaleItemResponse
        {
            Id = item.Id,
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Discount = item.UnitPrice * item.Quantity * (item.DiscountPercentage / 100m), // Calculando o valor do desconto
            TotalAmount = item.TotalItemAmount,
            IsCancelled = item.IsCancelled
        };
    }
}
