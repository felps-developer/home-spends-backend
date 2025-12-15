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

    /// <summary>
    /// Busca todas as transações de uma pessoa específica.
    /// Carrega o relacionamento Category para evitar consultas N+1.
    /// </summary>
    /// <param name="personId">Identificador único da pessoa.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de transações da pessoa com a categoria carregada.</returns>
    public async Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category) // Carrega a categoria para evitar consultas adicionais
            .Where(t => t.PersonId == personId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca todas as transações de uma categoria específica.
    /// Carrega o relacionamento Person para evitar consultas N+1.
    /// </summary>
    /// <param name="categoryId">Identificador único da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de transações da categoria com a pessoa carregada.</returns>
    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person) // Carrega a pessoa para evitar consultas adicionais
            .Where(t => t.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca todas as transações com todos os relacionamentos carregados.
    /// Carrega Person e Category para evitar consultas N+1 e permitir mapeamento completo para DTO.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de todas as transações com Person e Category carregados.</returns>
    public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)   // Carrega o relacionamento Person
            .Include(t => t.Category)  // Carrega o relacionamento Category
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca uma transação específica pelo ID com todos os relacionamentos carregados.
    /// Carrega Person e Category para evitar consultas N+1 e permitir mapeamento completo para DTO.
    /// </summary>
    /// <param name="id">Identificador único da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A transação encontrada com Person e Category carregados, ou null se não existir.</returns>
    public async Task<Transaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)   // Carrega o relacionamento Person
            .Include(t => t.Category) // Carrega o relacionamento Category
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}

