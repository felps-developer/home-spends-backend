namespace HomeSpends.Domain.Entities;

/// <summary>
/// Enum que define o tipo de transação.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Transação de despesa (gasto).
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Transação de receita (ganho).
    /// </summary>
    Income = 2
}

/// <summary>
/// Entidade que representa uma transação financeira.
/// Pode ser uma despesa ou receita, associada a uma pessoa e categoria.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Identificador único gerado automaticamente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da transação.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação (número decimal positivo).
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Tipo da transação (despesa ou receita).
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Identificador da categoria associada.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Navegação para a categoria.
    /// </summary>
    public virtual Category Category { get; set; } = null!;

    /// <summary>
    /// Identificador da pessoa associada.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Navegação para a pessoa.
    /// </summary>
    public virtual Person Person { get; set; } = null!;

    /// <summary>
    /// Data de criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data de atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

