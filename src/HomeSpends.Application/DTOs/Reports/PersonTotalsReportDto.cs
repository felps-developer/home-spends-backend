namespace HomeSpends.Application.DTOs.Reports;

/// <summary>
/// DTO para o relatório de totais por pessoa.
/// </summary>
public class PersonTotalsReportDto
{
    public List<PersonTotalsDto> People { get; set; } = new();
    public TotalsSummaryDto Summary { get; set; } = new();
}

