using HomeSpends.Application.DTOs.Reports;

namespace HomeSpends.Application.Services;

/// <summary>
/// Interface para o serviço de relatórios.
/// Define operações de negócio relacionadas a relatórios.
/// </summary>
public interface IReportService
{
    Task<PersonTotalsReportDto> GetPersonTotalsAsync(CancellationToken cancellationToken = default);
    Task<CategoryTotalsReportDto> GetCategoryTotalsAsync(CancellationToken cancellationToken = default);
}

