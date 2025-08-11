using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Models;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    Task<Sale?> GetByIdAsync(Guid id);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<PaginatedList<Sale>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task UpdateAsync(Sale sale);
}
