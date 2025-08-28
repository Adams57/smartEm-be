using SmartEM.Application.Utils;
using SmartEM.Domain;
using System.Linq.Expressions;

namespace SmartEM.Application.Contracts.Persistence;

public interface IPaginationService<TEntity, TKey> : IRepository<TEntity>
where TEntity : class, IEntity<TKey>
where TKey : IComparable<TKey>
{
    //Task<PaginatedResponse<TEntity>> GetAllAsync(PaginatedRequest? paginatedRequest);
    //Task<PaginatedResponse<TEntity>> GetAllIncludingAsync(PaginatedRequest? paginatedRequest, params Expression<Func<TEntity, object>>[] includeProperties);
    //Task<PaginatedResponse<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate, PaginatedRequest? paginatedRequest);
    //Task<PaginatedResponse<TEntity>> GetAllByPredicateIncluding(Expression<Func<TEntity, bool>> predicate, PaginatedRequest? paginatedRequest, params Expression<Func<TEntity, object>>[] includeProperties);
}
