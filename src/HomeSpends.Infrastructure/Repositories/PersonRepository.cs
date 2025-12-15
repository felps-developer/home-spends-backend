using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using HomeSpends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSpends.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de pessoas.
/// Fornece operações específicas para o domínio de pessoas.
/// </summary>
public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Person?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Transactions)
            .ThenInclude(t => t.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}

