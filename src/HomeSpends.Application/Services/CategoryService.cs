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
    private readonly IMapperService _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository repository,
        IMapperService mapper,
        ILogger<CategoryService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Busca todas as categorias cadastradas no sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de todas as categorias convertidas para DTO.</returns>
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as categorias");
            // Busca todas as categorias do repositório
            var categories = await _repository.GetAllAsync(cancellationToken);
            // Converte entidades para DTOs usando o serviço de mapeamento
            return categories.Select(_mapper.MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar categorias");
            throw;
        }
    }

    /// <summary>
    /// Busca uma categoria específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da categoria encontrada ou null se não existir.</returns>
    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando categoria com ID: {CategoryId}", id);
            // Busca categoria pelo ID
            var category = await _repository.GetByIdAsync(id, cancellationToken);
            // Retorna null se não encontrada, caso contrário converte para DTO
            return category != null ? _mapper.MapToDto(category) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar categoria com ID: {CategoryId}", id);
            throw;
        }
    }

    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// A categoria define uma finalidade (Purpose) que pode ser:
    /// - Expense: apenas para despesas
    /// - Income: apenas para receitas
    /// - Both: para ambas (despesas e receitas)
    /// </summary>
    /// <param name="dto">DTO com os dados da categoria a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da categoria criada.</returns>
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando nova categoria: {Description}", dto.Description);

            // Cria a entidade Category com os dados do DTO
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Purpose = dto.Purpose, // Define a finalidade da categoria
                CreatedAt = DateTime.UtcNow
            };

            // Persiste a categoria no banco de dados
            var created = await _repository.AddAsync(category, cancellationToken);
            _logger.LogInformation("Categoria criada com sucesso: {CategoryId}", created.Id);

            // Converte a entidade para DTO e retorna
            return _mapper.MapToDto(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar categoria");
            throw;
        }
    }
}

