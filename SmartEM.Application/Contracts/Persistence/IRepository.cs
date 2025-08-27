using System.Linq.Expressions;

namespace SmartEM.Application.Contracts.Persistence;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync<TKey>(TKey id);

    Task<TEntity> GetByIdIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<IReadOnlyList<TEntity>> GetAllAsync(bool asNoTracking = true);

    Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);

    Task<IReadOnlyList<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

    Task<IReadOnlyList<TEntity>> GetAllByPredicateIncluding(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TEntity> GetFirstOrDefaultBySpec(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TEntity> GetFirstOrDefaultByPredicateIncluding(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
}
