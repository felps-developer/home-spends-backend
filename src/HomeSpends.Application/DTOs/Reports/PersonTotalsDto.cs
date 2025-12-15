namespace HomeSpends.Application.DTOs.Reports;

/// <summary>
/// DTO para representar os totais de uma pessoa.
/// </summary>
public class PersonTotalsDto
{
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}

