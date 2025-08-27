using Microsoft.EntityFrameworkCore;
using SmartEM.Application.Contracts.Persistence;
using System.Linq.Expressions;

namespace SmartEM.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly SmartEMDbContext _dbContext;

    public Repository(SmartEMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(bool asNoTracking = true)
    {
        if (asNoTracking)
            return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
    {
        if (asNoTracking)
            return await _dbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByPredicateIncluding(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> entities;

        if (asNoTracking) entities = _dbContext.Set<TEntity>().Where(predicate).AsNoTracking();
        else
        {
            entities = _dbContext.Set<TEntity>().Where(predicate);
        }

        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return await entities.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync<TKey>(TKey id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> GetByIdIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var entity = _dbContext.Set<TEntity>().Where(predicate);
        var query = entity;

        foreach (var includeProperty in includeProperties)
        {
            if (includeProperty.Body is MemberExpression memberExpression)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.FirstOrDefaultAsync() ?? null!;
    }

    public async Task<TEntity> GetFirstOrDefaultBySpec(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken) ?? null!;
    }

    public async Task<TEntity> GetFirstOrDefaultByPredicateIncluding(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var entity = _dbContext.Set<TEntity>().Where(predicate);
        var query = entity;

        foreach (var includeProperty in includeProperties)
        {
            if (includeProperty.Body is MemberExpression memberExpression)
            {
                if (memberExpression.Type.IsGenericType && memberExpression.Type.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    query = query.Include(includeProperty);
                }
                else
                {
                    query = query.Include(includeProperty);
                }
            }
        }

        return await query.FirstOrDefaultAsync() ?? null!;
    }

    public async Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var entities = _dbContext.Set<TEntity>().AsNoTracking();
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }
        return await entities.ToListAsync();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        await Task.CompletedTask;
    }
}
