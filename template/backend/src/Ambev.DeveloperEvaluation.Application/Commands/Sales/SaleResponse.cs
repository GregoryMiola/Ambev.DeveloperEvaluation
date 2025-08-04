using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Commands.Sales;

public class SaleResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<SaleItemResponse> Items { get; set; } = new();

    public static SaleResponse FromSale(Sale sale)
    {
        return new SaleResponse
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber, 
            Date = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            IsCancelled = sale.IsCancelled,
            Items = sale.Items.Select(SaleItemResponse.FromSaleItem).ToList()
        };
    }
}