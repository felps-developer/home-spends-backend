using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using HomeSpends.Infrastructure.Data;

namespace HomeSpends.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de categorias.
/// Fornece operações específicas para o domínio de categorias.
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}

