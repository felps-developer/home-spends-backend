using HomeSpends.Application.DTOs.Transaction;

namespace HomeSpends.Application.Services;

/// <summary>
/// Interface para o serviço de transações.
/// Define operações de negócio relacionadas a transações.
/// </summary>
public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default);
}

