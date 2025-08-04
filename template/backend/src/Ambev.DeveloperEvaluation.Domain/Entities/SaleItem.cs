using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a line item within a sale.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        /// <summary>
        /// Gets the unique identifier for the sale this item belongs to.   
        /// </summary>
        public Guid SaleId { get; private set; }

        /// <summary>
        /// Gets the sale this item is part of.
        /// </summary>
        public Sale Sale { get; private set; } = null!;

        /// <summary>
        /// Gets the external identifier for the product.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the denormalized product name at the time of sale.
        /// </summary>
        public string ProductName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the quantity of the product sold.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the unit price of the product at the time of sale (snapshot).
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the discount percentage applied to this item.
        /// </summary>
        public decimal DiscountPercentage { get; private set; }

        /// <summary>
        /// Gets the total amount for this item, calculated as:
        /// <c>Quantity * UnitPrice * (1 - DiscountPercentage / 100)</c>.
        /// </summary>
        public decimal TotalItemAmount { get; private set; }

        /// <summary>
        /// Indicates whether the item has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }
    }
}