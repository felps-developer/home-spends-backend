using HomeSpends.Domain.Entities;

namespace HomeSpends.Domain.Interfaces;

/// <summary>
/// Interface específica para repositório de transações.
/// Define operações específicas do domínio de transações.
/// </summary>
public interface ITransactionRepository : IRepository<Transaction>
{
    /// <summary>
    /// Busca transações de uma pessoa específica.
    /// </summary>
    Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca transações de uma categoria específica.
    /// </summary>
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca transações com informações de pessoa e categoria.
    /// </summary>
    Task<IEnumerable<Transaction>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca uma transação pelo ID com informações de pessoa e categoria.
    /// </summary>
    Task<Transaction?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}

