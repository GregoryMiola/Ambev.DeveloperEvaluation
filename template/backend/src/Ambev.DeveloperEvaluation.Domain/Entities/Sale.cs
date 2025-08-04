using Ambev.DeveloperEvaluation.Domain.Exceptions;

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
        public string SaleNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the date and time the sale was made.
        /// </summary>
        public DateTime SaleDate { get; private set; }

        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;

        public Guid BranchId { get; private set; }

        public decimal TotalAmount { get; private set; }

        public bool IsCancelled { get; private set; }

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        // Private constructor for EF Core
        private Sale() { }

        public static Sale Create(Guid customerId, string customerName, Guid branchId)
        {
            return new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = $"SALE-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                SaleDate = DateTime.UtcNow,
                IsCancelled = false,
                CustomerId = customerId,
                CustomerName = customerName, // Denormalized
                BranchId = branchId
            };
        }

        public void AddItem(Product product, int quantity)
        {
            var newItem = SaleItem.Create(this.Id, product, quantity);
            _items.Add(newItem);
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Where(i => !i.IsCancelled).Sum(item => item.TotalItemAmount);
        }

        public void RemoveItem(Guid itemId)
        {
            if (IsCancelled)
            {
                throw new DomainException("Não é possível remover itens de uma venda cancelada.");
            }

            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new DomainException($"Item com ID {itemId} não encontrado nesta venda.");

            item.Cancel();
            RecalculateTotal();
        }

        public void Cancel()
        {
            if (IsCancelled)
            {
                throw new DomainException("A venda já está cancelada.");
            }

            IsCancelled = true;
            foreach (var item in _items)
            {
                if (!item.IsCancelled)
                {
                    item.Cancel();
                }
            }
            RecalculateTotal();
        }
    }
}