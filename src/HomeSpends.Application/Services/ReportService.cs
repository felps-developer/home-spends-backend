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

    public async Task<PersonTotalsReportDto> GetPersonTotalsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de totais por pessoa");

            var people = await _personRepository.GetAllAsync(cancellationToken);
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);

            var personTotals = people.Select(person =>
            {
                var personTransactions = transactions.Where(t => t.PersonId == person.Id).ToList();

                return new PersonTotalsDto
                {
                    PersonId = person.Id,
                    PersonName = person.Name,
                    TotalIncome = personTransactions
                        .Where(t => t.Type == TransactionType.Income)
                        .Sum(t => t.Value),
                    TotalExpense = personTransactions
                        .Where(t => t.Type == TransactionType.Expense)
                        .Sum(t => t.Value)
                };
            }).ToList();

            var summary = new TotalsSummaryDto
            {
                TotalIncome = personTotals.Sum(p => p.TotalIncome),
                TotalExpense = personTotals.Sum(p => p.TotalExpense)
            };

            _logger.LogInformation("Relatório de totais por pessoa gerado com sucesso");

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

    public async Task<CategoryTotalsReportDto> GetCategoryTotalsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de totais por categoria");

            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            var transactions = await _transactionRepository.GetAllWithDetailsAsync(cancellationToken);

            var categoryTotals = categories.Select(category =>
            {
                var categoryTransactions = transactions.Where(t => t.CategoryId == category.Id).ToList();

                return new CategoryTotalsDto
                {
                    CategoryId = category.Id,
                    CategoryDescription = category.Description,
                    TotalIncome = categoryTransactions
                        .Where(t => t.Type == TransactionType.Income)
                        .Sum(t => t.Value),
                    TotalExpense = categoryTransactions
                        .Where(t => t.Type == TransactionType.Expense)
                        .Sum(t => t.Value)
                };
            }).ToList();

            var summary = new TotalsSummaryDto
            {
                TotalIncome = categoryTotals.Sum(c => c.TotalIncome),
                TotalExpense = categoryTotals.Sum(c => c.TotalExpense)
            };

            _logger.LogInformation("Relatório de totais por categoria gerado com sucesso");

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

