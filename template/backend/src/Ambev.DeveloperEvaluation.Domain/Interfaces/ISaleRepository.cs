using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    // Outros métodos como GetByIdAsync, UpdateAsync, etc.

    Task<Sale?> GetByIdAsync(Guid id);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task UpdateAsync(Sale sale);
}
