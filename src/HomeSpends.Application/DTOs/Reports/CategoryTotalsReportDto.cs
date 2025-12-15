namespace HomeSpends.Application.DTOs.Reports;

/// <summary>
/// DTO para o relatório de totais por categoria.
/// </summary>
public class CategoryTotalsReportDto
{
    public List<CategoryTotalsDto> Categories { get; set; } = new();
    public TotalsSummaryDto Summary { get; set; } = new();
}

