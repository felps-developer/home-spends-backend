using System.ComponentModel.DataAnnotations;
using HomeSpends.Domain.Entities;

namespace HomeSpends.Application.DTOs.Category;

/// <summary>
/// DTO para criação de uma categoria.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Finalidade da categoria (despesa/receita/ambas).
    /// </summary>
    [Required(ErrorMessage = "A finalidade é obrigatória")]
    [Range(1, 3, ErrorMessage = "A finalidade deve ser: 1 (Despesa), 2 (Receita) ou 3 (Ambas)")]
    public CategoryPurpose Purpose { get; set; }
}

