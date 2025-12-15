using HomeSpends.Domain.Entities;

namespace HomeSpends.Application.DTOs.Category;

/// <summary>
/// DTO para representação de uma categoria.
/// </summary>
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public CategoryPurpose Purpose { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

