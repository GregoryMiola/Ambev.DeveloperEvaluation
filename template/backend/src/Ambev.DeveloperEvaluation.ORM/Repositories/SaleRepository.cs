using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
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
        // Trocamos FindAsync por FirstOrDefaultAsync para podermos usar o .Include()
        // e carregar os itens relacionados.
        return await _context.Sales
                             .Include(s => s.Items)
                             .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        // Adicionamos .Include() para garantir que todas as vendas na lista
        // venham com seus respectivos itens.
        return await _context.Sales.Include(s => s.Items).ToListAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }
}
