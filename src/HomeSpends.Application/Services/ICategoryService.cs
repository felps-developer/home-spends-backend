using HomeSpends.Application.DTOs.Category;

namespace HomeSpends.Application.Services;

/// <summary>
/// Interface para o serviço de categorias.
/// Define operações de negócio relacionadas a categorias.
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
}

