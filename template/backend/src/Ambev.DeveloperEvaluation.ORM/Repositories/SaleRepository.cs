using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Common.Models;
using Microsoft.EntityFrameworkCore; 

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
                             .Include(s => s.Items)
                             .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales.Include(s => s.Items).ToListAsync();
    }

    public async Task<PaginatedList<Sale>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Sales
                            .Include(s => s.Items)
                            .OrderByDescending(s => s.SaleDate)
                            .AsNoTracking();
                            
        return await PaginatedList<Sale>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }
}
