using System.Linq.Expressions;
using HomeSpends.Domain.Interfaces;
using HomeSpends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeSpends.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica do padrão Repository.
/// Fornece operações básicas de CRUD usando Entity Framework Core.
/// </summary>
/// <typeparam name="T">Tipo da entidade.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Busca uma entidade pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da entidade.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A entidade encontrada ou null se não existir.</returns>
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    /// <summary>
    /// Busca todas as entidades do tipo T.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de todas as entidades encontradas.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Busca entidades que atendem a um predicado específico.
    /// </summary>
    /// <param name="predicate">Expressão lambda que define a condição de busca.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de entidades que atendem ao predicado.</returns>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adiciona uma nova entidade ao banco de dados.
    /// Persiste as alterações imediatamente (SaveChanges).
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>A entidade adicionada (com ID gerado, se aplicável).</returns>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Atualiza uma entidade existente no banco de dados.
    /// Persiste as alterações imediatamente (SaveChanges).
    /// </summary>
    /// <param name="entity">Entidade a ser atualizada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deleta uma entidade do banco de dados.
    /// Persiste as alterações imediatamente (SaveChanges).
    /// </summary>
    /// <param name="entity">Entidade a ser deletada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deleta uma entidade pelo seu identificador único.
    /// Busca a entidade primeiro e, se encontrada, a deleta.
    /// </summary>
    /// <param name="id">Identificador único da entidade a ser deletada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            await DeleteAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Verifica se existe pelo menos uma entidade que atende ao predicado.
    /// </summary>
    /// <param name="predicate">Expressão lambda que define a condição de verificação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>True se existe pelo menos uma entidade que atende ao predicado, False caso contrário.</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }
}

