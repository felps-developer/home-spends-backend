using System.ComponentModel.DataAnnotations;

namespace HomeSpends.Application.DTOs.Transaction;

/// <summary>
/// DTO para criação de uma transação.
/// </summary>
public class CreateTransactionDto
{
    /// <summary>
    /// Descrição da transação.
    /// </summary>
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação (número decimal positivo).
    /// </summary>
    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número decimal positivo")]
    public decimal Value { get; set; }

    /// <summary>
    /// Tipo da transação (despesa ou receita).
    /// </summary>
    [Required(ErrorMessage = "O tipo é obrigatório")]
    public Domain.Entities.TransactionType Type { get; set; }

    /// <summary>
    /// Identificador da categoria.
    /// </summary>
    [Required(ErrorMessage = "A categoria é obrigatória")]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Identificador da pessoa.
    /// </summary>
    [Required(ErrorMessage = "A pessoa é obrigatória")]
    public Guid PersonId { get; set; }
}

