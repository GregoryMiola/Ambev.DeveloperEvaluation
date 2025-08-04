using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Gets the user-facing sale number.
        /// </summary>
        public int SaleNumber { get; private set; }

        /// <summary>
        /// Gets the date and time the sale was made.
        /// </summary>
        public DateTime SaleDate { get; private set; }

        /// <summary>
        /// Gets the external identifier for the customer.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Gets the denormalized customer name at the time of sale.
        /// </summary>
        public string CustomerName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the name of the branch where the sale occurred.
        /// </summary>
        public string BranchName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the total amount of the sale, including all items and discounts.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Indicates whether the sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Represents the collection of items in the sale.
        /// </summary>
        public ICollection<SaleItem> Items { get; private set; } = [];
    }
}