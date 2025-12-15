using HomeSpends.Application.DTOs.Category;
using HomeSpends.Application.DTOs.Person;
using HomeSpends.Domain.Entities;

namespace HomeSpends.Application.DTOs.Transaction;

/// <summary>
/// DTO para representação de uma transação.
/// </summary>
public class TransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public TransactionType Type { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public PersonDto Person { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

