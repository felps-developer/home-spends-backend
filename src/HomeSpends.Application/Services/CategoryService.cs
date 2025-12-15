using HomeSpends.Application.DTOs.Category;
using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeSpends.Application.Services;

/// <summary>
/// Serviço de aplicação para categorias.
/// Implementa as regras de negócio relacionadas a categorias.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as categorias");
            var categories = await _repository.GetAllAsync(cancellationToken);
            return categories.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar categorias");
            throw;
        }
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando categoria com ID: {CategoryId}", id);
            var category = await _repository.GetByIdAsync(id, cancellationToken);
            return category != null ? MapToDto(category) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar categoria com ID: {CategoryId}", id);
            throw;
        }
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando nova categoria: {Description}", dto.Description);

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Purpose = dto.Purpose,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(category, cancellationToken);
            _logger.LogInformation("Categoria criada com sucesso: {CategoryId}", created.Id);

            return MapToDto(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar categoria");
            throw;
        }
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Description = category.Description,
            Purpose = category.Purpose,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}

