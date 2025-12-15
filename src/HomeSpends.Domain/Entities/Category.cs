namespace HomeSpends.Domain.Entities;

/// <summary>
/// Enum que define a finalidade de uma categoria.
/// </summary>
public enum CategoryPurpose
{
    /// <summary>
    /// Categoria apenas para despesas.
    /// </summary>
    Expense = 1,

    /// <summary>
    /// Categoria apenas para receitas.
    /// </summary>
    Income = 2,

    /// <summary>
    /// Categoria para ambas (despesas e receitas).
    /// </summary>
    Both = 3
}

/// <summary>
/// Entidade que representa uma categoria de transação.
/// Define a finalidade (despesa/receita/ambas) para restringir o uso em transações.
/// </summary>
public class Category
{
    /// <summary>
    /// Identificador único gerado automaticamente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria (despesa/receita/ambas).
    /// </summary>
    public CategoryPurpose Purpose { get; set; }

    /// <summary>
    /// Data de criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data de atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Navegação para as transações da categoria.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// Verifica se a categoria pode ser usada para despesas.
    /// </summary>
    public bool CanBeUsedForExpense => Purpose == CategoryPurpose.Expense || Purpose == CategoryPurpose.Both;

    /// <summary>
    /// Verifica se a categoria pode ser usada para receitas.
    /// </summary>
    public bool CanBeUsedForIncome => Purpose == CategoryPurpose.Income || Purpose == CategoryPurpose.Both;
}

