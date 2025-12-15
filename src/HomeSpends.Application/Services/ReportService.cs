using HomeSpends.Application.DTOs.Reports;
using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeSpends.Application.Services;

/// <summary>
/// Serviço de aplicação para relatórios.
/// Implementa as regras de negócio relacionadas a relatórios.
/// </summary>
public class ReportService : IReportService
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ILogger<ReportService> logger)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gera um relatório de totais agrupados por pessoa.
    /// Para cada pessoa, calcula:
    /// - Total de receitas (Income)
    /// - Total de despesas (Expense)
    /// - Saldo (receitas - despesas)
    /// Ao final, calcula o total geral de todas as pessoas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO contendo os totais por pessoa e o resumo geral.</returns>
    public async Task<PersonTotalsReportDto> GetPersonTotalsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de totais por pessoa");

            // Busca todas as pessoas e transações do sistema
            var people = await _personRepository.GetAllAsync(cancellationToken);
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);

            // Para cada pessoa, calcula os totais de receitas e despesas
            var personTotals = people.Select(person =>
            {
                // Filtra transações da pessoa atual
                var personTransactions = transactions.Where(t => t.PersonId == person.Id).ToList();

                // Cria DTO com os totais calculados
                return new PersonTotalsDto
                {
                    PersonId = person.Id,
                    PersonName = person.Name,
                    // Soma apenas transações do tipo Income (receitas)
                    TotalIncome = personTransactions
                        .Where(t => t.Type == TransactionType.Income)
                        .Sum(t => t.Value),
                    // Soma apenas transações do tipo Expense (despesas)
                    TotalExpense = personTransactions
                        .Where(t => t.Type == TransactionType.Expense)
                        .Sum(t => t.Value)
                    // O saldo (balance) é calculado automaticamente no DTO: TotalIncome - TotalExpense
                };
            }).ToList();

            // Calcula o resumo geral somando todos os totais individuais
            var summary = new TotalsSummaryDto
            {
                TotalIncome = personTotals.Sum(p => p.TotalIncome),
                TotalExpense = personTotals.Sum(p => p.TotalExpense)
                // O saldo líquido (netBalance) é calculado automaticamente no DTO: TotalIncome - TotalExpense
            };

            _logger.LogInformation("Relatório de totais por pessoa gerado com sucesso");

            // Retorna o relatório completo com totais por pessoa e resumo geral
            return new PersonTotalsReportDto
            {
                People = personTotals,
                Summary = summary
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de totais por pessoa");
            throw;
        }
    }

    /// <summary>
    /// Gera um relatório de totais agrupados por categoria.
    /// Para cada categoria, calcula:
    /// - Total de receitas (Income)
    /// - Total de despesas (Expense)
    /// - Saldo (receitas - despesas)
    /// Ao final, calcula o total geral de todas as categorias.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO contendo os totais por categoria e o resumo geral.</returns>
    public async Task<CategoryTotalsReportDto> GetCategoryTotalsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de totais por categoria");

            // Busca todas as categorias e transações do sistema
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);

            // Para cada categoria, calcula os totais de receitas e despesas
            var categoryTotals = categories.Select(category =>
            {
                // Filtra transações da categoria atual
                var categoryTransactions = transactions.Where(t => t.CategoryId == category.Id).ToList();

                // Cria DTO com os totais calculados
                return new CategoryTotalsDto
                {
                    CategoryId = category.Id,
                    CategoryDescription = category.Description,
                    // Soma apenas transações do tipo Income (receitas)
                    TotalIncome = categoryTransactions
                        .Where(t => t.Type == TransactionType.Income)
                        .Sum(t => t.Value),
                    // Soma apenas transações do tipo Expense (despesas)
                    TotalExpense = categoryTransactions
                        .Where(t => t.Type == TransactionType.Expense)
                        .Sum(t => t.Value)
                    // O saldo (balance) é calculado automaticamente no DTO: TotalIncome - TotalExpense
                };
            }).ToList();

            // Calcula o resumo geral somando todos os totais individuais
            var summary = new TotalsSummaryDto
            {
                TotalIncome = categoryTotals.Sum(c => c.TotalIncome),
                TotalExpense = categoryTotals.Sum(c => c.TotalExpense)
                // O saldo líquido (netBalance) é calculado automaticamente no DTO: TotalIncome - TotalExpense
            };

            _logger.LogInformation("Relatório de totais por categoria gerado com sucesso");

            // Retorna o relatório completo com totais por categoria e resumo geral
            return new CategoryTotalsReportDto
            {
                Categories = categoryTotals,
                Summary = summary
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de totais por categoria");
            throw;
        }
    }
}

