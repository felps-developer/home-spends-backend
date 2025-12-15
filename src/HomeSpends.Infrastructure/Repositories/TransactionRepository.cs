using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using HomeSpends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSpends.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de transações.
/// Fornece operações específicas para o domínio de transações.
/// </summary>
public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.PersonId == personId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<Transaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}

