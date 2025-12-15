using HomeSpends.Application.DTOs.Category;
using HomeSpends.Application.DTOs.Person;
using HomeSpends.Application.DTOs.Transaction;
using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeSpends.Application.Services;

/// <summary>
/// Serviço de aplicação para transações.
/// Implementa as regras de negócio relacionadas a transações.
/// </summary>
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IPersonRepository personRepository,
        ICategoryRepository categoryRepository,
        ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as transações");
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);
            return transactions.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transações");
            throw;
        }
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando transação com ID: {TransactionId}", id);
            var transaction = await _transactionRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            return transaction != null ? MapToDto(transaction) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transação com ID: {TransactionId}", id);
            throw;
        }
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando nova transação para pessoa: {PersonId}", dto.PersonId);

            // Validação: Verificar se a pessoa existe
            var person = await _personRepository.GetByIdAsync(dto.PersonId, cancellationToken);
            if (person == null)
            {
                throw new InvalidOperationException($"Pessoa com ID {dto.PersonId} não encontrada");
            }

            // Regra de negócio: Menores de idade só podem ter despesas
            if (person.IsMinor && dto.Type != TransactionType.Expense)
            {
                throw new InvalidOperationException("Menores de idade (menor de 18 anos) só podem ter despesas");
            }

            // Validação: Verificar se a categoria existe
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId, cancellationToken);
            if (category == null)
            {
                throw new InvalidOperationException($"Categoria com ID {dto.CategoryId} não encontrada");
            }

            // Regra de negócio: Validar se a categoria pode ser usada para o tipo de transação
            if (dto.Type == TransactionType.Expense && !category.CanBeUsedForExpense)
            {
                throw new InvalidOperationException(
                    $"A categoria '{category.Description}' não pode ser usada para despesas. Finalidade: {category.Purpose}");
            }

            if (dto.Type == TransactionType.Income && !category.CanBeUsedForIncome)
            {
                throw new InvalidOperationException(
                    $"A categoria '{category.Description}' não pode ser usada para receitas. Finalidade: {category.Purpose}");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Value = dto.Value,
                Type = dto.Type,
                CategoryId = dto.CategoryId,
                PersonId = dto.PersonId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _transactionRepository.AddAsync(transaction, cancellationToken);

            // Carregar relacionamentos para retornar DTO completo
            var transactionWithDetails = await _transactionRepository.GetByIdWithDetailsAsync(created.Id, cancellationToken);
            if (transactionWithDetails == null)
            {
                throw new InvalidOperationException("Erro ao recuperar transação criada");
            }

            _logger.LogInformation("Transação criada com sucesso: {TransactionId}", created.Id);

            return MapToDto(transactionWithDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            throw;
        }
    }

    private static TransactionDto MapToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Value = transaction.Value,
            Type = transaction.Type,
            Category = new CategoryDto
            {
                Id = transaction.Category.Id,
                Description = transaction.Category.Description,
                Purpose = transaction.Category.Purpose,
                CreatedAt = transaction.Category.CreatedAt,
                UpdatedAt = transaction.Category.UpdatedAt
            },
            Person = new PersonDto
            {
                Id = transaction.Person.Id,
                Name = transaction.Person.Name,
                Age = transaction.Person.Age,
                CreatedAt = transaction.Person.CreatedAt,
                UpdatedAt = transaction.Person.UpdatedAt
            },
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}

