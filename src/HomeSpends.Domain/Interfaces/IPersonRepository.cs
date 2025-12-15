using HomeSpends.Domain.Entities;

namespace HomeSpends.Domain.Interfaces;

/// <summary>
/// Interface específica para repositório de pessoas.
/// Define operações específicas do domínio de pessoas.
/// </summary>
public interface IPersonRepository : IRepository<Person>
{
    /// <summary>
    /// Busca uma pessoa com suas transações.
    /// </summary>
    Task<Person?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
}

