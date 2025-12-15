namespace HomeSpends.Application.DTOs.Reports;

/// <summary>
/// DTO para representar o resumo geral de totais.
/// </summary>
public class TotalsSummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetBalance => TotalIncome - TotalExpense;
}

