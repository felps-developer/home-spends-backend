namespace HomeSpends.Domain.Entities;

/// <summary>
/// Entidade que representa uma pessoa no sistema.
/// Contém informações básicas de identificação e idade.
/// </summary>
public class Person
{
    /// <summary>
    /// Identificador único gerado automaticamente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa (número inteiro positivo).
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Data de criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data de atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Navegação para as transações da pessoa.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// Verifica se a pessoa é menor de idade (menor que 18 anos).
    /// </summary>
    public bool IsMinor => Age < 18;
}

