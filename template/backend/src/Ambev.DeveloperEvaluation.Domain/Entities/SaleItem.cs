using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a line item within a sale.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        public Guid SaleId { get; private set; }
        public Sale Sale { get; private set; } = null!;

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;

        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal DiscountPercentage { get; private set; }

        public decimal TotalItemAmount { get; private set; }

        public bool IsCancelled { get; private set; }

        // Private constructor for EF Core
        private SaleItem() { }

        public static SaleItem Create(Guid saleId, Product product, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("A quantidade do item deve ser maior que zero.");

            if (quantity > 20)
                throw new DomainException($"Não é possível vender mais de 20 itens do produto '{product.Name}'. Quantidade solicitada: {quantity}.");

            decimal discountPercentage = 0m;
            if (quantity >= 10) // 10-20
            {
                discountPercentage = 20m; // 20%
            }
            else if (quantity >= 4) // 4-9
            {
                discountPercentage = 10m; // 10%
            }

            var item = new SaleItem
            {
                Id = Guid.NewGuid(),
                SaleId = saleId,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = quantity,
                DiscountPercentage = discountPercentage,
                IsCancelled = false
            };

            item.TotalItemAmount = (item.UnitPrice * item.Quantity) * (1 - (item.DiscountPercentage / 100m));
            return item;
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Este item já foi cancelado.");

            IsCancelled = true;
            TotalItemAmount = 0; // Reset total amount when cancelled
        }
    }
}