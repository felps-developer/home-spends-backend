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
    private readonly IMapperService _mapper;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IPersonRepository personRepository,
        ICategoryRepository categoryRepository,
        IMapperService mapper,
        ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Busca todas as transações cadastradas no sistema.
    /// Carrega os relacionamentos (Pessoa e Categoria) para retornar dados completos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de todas as transações convertidas para DTO.</returns>
    public async Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as transações");
            // Busca transações com relacionamentos carregados (Include)
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);
            // Converte entidades para DTOs usando o serviço de mapeamento
            return transactions.Select(_mapper.MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transações");
            throw;
        }
    }

    /// <summary>
    /// Busca uma transação específica pelo seu identificador único.
    /// Carrega os relacionamentos (Pessoa e Categoria) para retornar dados completos.
    /// </summary>
    /// <param name="id">Identificador único da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da transação encontrada ou null se não existir.</returns>
    public async Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando transação com ID: {TransactionId}", id);
            // Busca transação com relacionamentos carregados
            var transaction = await _transactionRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            // Retorna null se não encontrada, caso contrário converte para DTO
            return transaction != null ? _mapper.MapToDto(transaction) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transação com ID: {TransactionId}", id);
            throw;
        }
    }

    /// <summary>
    /// Cria uma nova transação no sistema.
    /// Aplica validações e regras de negócio:
    /// - Verifica se a pessoa existe
    /// - Verifica se a categoria existe
    /// - Valida se menores de idade só podem ter despesas (não receitas)
    /// - Valida se a categoria permite o tipo de transação (despesa/receita)
    /// </summary>
    /// <param name="dto">DTO com os dados da transação a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da transação criada com todos os relacionamentos carregados.</returns>
    /// <exception cref="InvalidOperationException">Lançada quando alguma validação de negócio falha.</exception>
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

            // Regra de negócio: Menores de idade (menor de 18 anos) só podem ter despesas
            // Esta é uma regra de negócio importante que restringe receitas para menores
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
            // Cada categoria tem uma finalidade (Expense, Income ou Both) que restringe seu uso
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

            // Cria a entidade Transaction com os dados do DTO
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

            // Persiste a transação no banco de dados
            var created = await _transactionRepository.AddAsync(transaction, cancellationToken);

            // Carregar relacionamentos (Person e Category) para retornar DTO completo
            // Isso é necessário porque o AddAsync não carrega automaticamente os relacionamentos
            var transactionWithDetails = await _transactionRepository.GetByIdWithDetailsAsync(created.Id, cancellationToken);
            if (transactionWithDetails == null)
            {
                throw new InvalidOperationException("Erro ao recuperar transação criada");
            }

            _logger.LogInformation("Transação criada com sucesso: {TransactionId}", created.Id);

            // Converte a entidade com relacionamentos para DTO e retorna
            return _mapper.MapToDto(transactionWithDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            throw;
        }
    }
}

