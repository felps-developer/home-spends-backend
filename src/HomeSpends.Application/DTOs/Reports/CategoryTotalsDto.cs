namespace HomeSpends.Application.DTOs.Reports;

/// <summary>
/// DTO para representar os totais de uma categoria.
/// </summary>
public class CategoryTotalsDto
{
    public Guid CategoryId { get; set; }
    public string CategoryDescription { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}

