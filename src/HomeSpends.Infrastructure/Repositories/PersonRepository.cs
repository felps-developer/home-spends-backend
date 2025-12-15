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

    /// <summary>
    /// Busca uma pessoa específica pelo ID com todas as suas transações carregadas.
    /// Carrega o relacionamento Transactions e, para cada transação, carrega também a Category.
    /// Útil para exibir detalhes completos de uma pessoa e suas transações.
    /// </summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A pessoa encontrada com todas as transações e suas categorias carregadas, ou null se não existir.</returns>
    public async Task<Person?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Transactions)           // Carrega todas as transações da pessoa
            .ThenInclude(t => t.Category)            // Para cada transação, carrega também a categoria
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}

