using System.ComponentModel.DataAnnotations;

namespace HomeSpends.Application.DTOs.Person;

/// <summary>
/// DTO para criação de uma pessoa.
/// </summary>
public class CreatePersonDto
{
    /// <summary>
    /// Nome da pessoa.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa (número inteiro positivo).
    /// </summary>
    [Required(ErrorMessage = "A idade é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "A idade deve ser um número inteiro positivo")]
    public int Age { get; set; }
}

