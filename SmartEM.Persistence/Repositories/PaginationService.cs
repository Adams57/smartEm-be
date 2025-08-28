using Microsoft.EntityFrameworkCore;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;
using SmartEM.Domain;
using System.Linq.Expressions;

namespace SmartEM.Persistence.Repositories;

public class PaginationService<TEntity, TId> : Repository<TEntity>, IPaginationService<TEntity, TId>
where TEntity : class, IEntity<TId>
where TId : IComparable<TId>
{
    protected readonly SmartEMDbContext _dbContext;
    private const int MAX_PAGE_SIZE = 100;

    public PaginationService(SmartEMDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    //public async Task<PaginatedResponse<TEntity>> GetAllAsync(PaginatedRequest? paginatedRequest)
    //{
    //    var query = _dbContext.Set<TEntity>().AsQueryable();
    //    return await PaginateResponse(query, paginatedRequest);
    //}

    //public async Task<PaginatedResponse<TEntity>> GetAllByPredicate(Expression<Func<TEntity, bool>> predicate,
    //    PaginatedRequest? paginatedRequest = null!)
    //{
    //    var query = _dbContext.Set<TEntity>().Where(predicate).AsQueryable();
    //    return await PaginateResponse(query, paginatedRequest);
    //}

    //public async Task<PaginatedResponse<TEntity>> GetAllByPredicateIncluding(Expression<Func<TEntity, bool>> predicate,
    //    PaginatedRequest? paginatedRequest = null!, params Expression<Func<TEntity, object>>[] includeProperties)
    //{
    //    IQueryable<TEntity> entities = _dbContext.Set<TEntity>().Where(predicate).AsQueryable();

    //    foreach (var includeProperty in includeProperties)
    //    {
    //        entities = entities.Include(includeProperty);
    //    }

    //    return await PaginateResponse(entities, paginatedRequest);
    //}

    //public async Task<PaginatedResponse<TEntity>> GetAllIncludingAsync(PaginatedRequest? paginatedRequest = null!, params Expression<Func<TEntity, object>>[] includeProperties)
    //{
    //    var entities = _dbContext.Set<TEntity>().AsQueryable();
    //    foreach (var includeProperty in includeProperties)
    //    {
    //        entities = entities.Include(includeProperty);
    //    }
    //    return await PaginateResponse(entities, paginatedRequest);
    //}

    //protected async Task<PaginatedResponse<TEntity>> PaginateResponse(
    //    IQueryable<TEntity> query,
    //    PaginatedRequest? paginatedRequest = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    if (paginatedRequest == null)
    //    {
    //        var data = await query.OrderBy(x => x.SequentialId).AsNoTracking().ToListAsync(cancellationToken);
    //        (int forwardLastKey, int backwardLastKey) = data.Count > 0 ? (data.Last().SequentialId, data.First().SequentialId) : (default, default);
    //        return new PaginatedResponse<TEntity>(data.Count, forwardLastKey, backwardLastKey, data, data.Count, 1);
    //    }

    //    var pageSize_ = paginatedRequest.PageSize <= MAX_PAGE_SIZE ? paginatedRequest.PageSize : MAX_PAGE_SIZE;
    //    var totalCount = await query.CountAsync(cancellationToken);

    //    if (paginatedRequest.UseLastKey())
    //        return await PaginateEntityWithLastKey(query, paginatedRequest, pageSize_, totalCount, cancellationToken);

    //    return await PaginateEntityByOffset(query, paginatedRequest, pageSize_, totalCount, cancellationToken);
    //}

    //private async Task<PaginatedResponse<TEntity>> PaginateEntityByOffset(IQueryable<TEntity> query, PaginatedRequest paginatedRequest, int pageSize_, int totalCount, CancellationToken cancellationToken)
    //{
    //    int pageNumber_ = paginatedRequest.PageNumber.GetValueOrDefault() <= 0 ? 1 : paginatedRequest.PageNumber.GetValueOrDefault();
    //    int skip = (pageNumber_ - 1) * pageSize_;
    //    var paginatedData = await query.OrderBy(e => e.SequentialId)
    //        .Skip(skip)
    //        .Take(pageSize_).ToListAsync(cancellationToken);

    //    (int forwardLastKey, int backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.Last().SequentialId, paginatedData.First().SequentialId) : (default, default);
    //    return new PaginatedResponse<TEntity>(pageSize_, forwardLastKey, backwardLastKey, paginatedData, totalCount);
    //}

    //private async Task<PaginatedResponse<TEntity>> PaginateEntityWithLastKey(IQueryable<TEntity> query,
    //    PaginatedRequest paginatedRequest, int pageSize, int totalCount, CancellationToken cancellationToken)
    //{
    //    List<TEntity> paginatedData;
    //    int forwardLastKey = paginatedRequest.ForwardLastKey.GetValueOrDefault();
    //    int backwardLastKey = paginatedRequest.BackwardLastKey.GetValueOrDefault();

    //    if (paginatedRequest.IsBackward)
    //    {
    //        query = query.Where(x => x.SequentialId < backwardLastKey)
    //            .OrderByDescending(e => e.SequentialId);
    //        paginatedData = await query.AsNoTracking().Take(pageSize).ToListAsync(cancellationToken);
    //        (forwardLastKey, backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.First().SequentialId, paginatedData.Last().SequentialId) : (default, default);
    //        return new PaginatedResponse<TEntity>(pageSize, paginatedData, totalCount, paginatedRequest.PageNumber.GetValueOrDefault(), forwardLastKey, backwardLastKey);
    //    }

    //    query = query.Where(x => x.SequentialId > forwardLastKey).OrderBy(e => e.SequentialId);
    //    paginatedData = await query.AsNoTracking().Take(pageSize).ToListAsync(cancellationToken);
    //    (forwardLastKey, backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.Last().SequentialId, paginatedData.First().SequentialId) : (default, default);
    //    return new PaginatedResponse<TEntity>(pageSize, paginatedData, totalCount, paginatedRequest.PageNumber.GetValueOrDefault(), forwardLastKey, backwardLastKey);
    //}

    //protected async Task<PaginatedResponse<TProjection>> PaginateResponse<TProjection>(
    //    IQueryable<TProjection> query,
    //    PaginatedRequest? paginatedRequest,
    //    CancellationToken cancellationToken = default) where TProjection : class, IProjectionDto
    //{
    //    if (paginatedRequest == null)
    //    {
    //        var data = await query.OrderBy(x => x.SequentialId).ToListAsync(cancellationToken);
    //        (int forwardLastKey, int backwardLastKey) = data.Count > 0 ? (data.Last().SequentialId, data.First().SequentialId) : (default, default);
    //        return new PaginatedResponse<TProjection>(data.Count, forwardLastKey, backwardLastKey, data, data.Count, 1);
    //    }

    //    var pageSize_ = paginatedRequest.PageSize <= MAX_PAGE_SIZE ? paginatedRequest.PageSize : MAX_PAGE_SIZE;
    //    var totalCount = await query.CountAsync(cancellationToken);

    //    if (paginatedRequest.UseLastKey())
    //        return await PaginateProjectionWithLastKey(query, paginatedRequest, pageSize_, totalCount, cancellationToken);

    //    return await PaginateProjectionByOffset(query, paginatedRequest, pageSize_, totalCount, cancellationToken);
    //}

    //private async Task<PaginatedResponse<TProjection>> PaginateProjectionByOffset<TProjection>(IQueryable<TProjection> query, PaginatedRequest paginatedRequest, int pageSize_, int totalCount, CancellationToken cancellationToken) where TProjection : class, IProjectionDto
    //{
    //    int pageNumber_ = paginatedRequest.PageNumber.GetValueOrDefault() <= 0 ? 1 : paginatedRequest.PageNumber.GetValueOrDefault();
    //    int skip = (pageNumber_ - 1) * pageSize_;
    //    var paginatedData = await query.OrderBy(e => e.SequentialId)
    //        .Skip(skip)
    //        .Take(pageSize_).ToListAsync(cancellationToken);

    //    (int forwardLastKey, int backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.Last().SequentialId, paginatedData.First().SequentialId) : (default, default);
    //    return new PaginatedResponse<TProjection>(pageSize_, forwardLastKey, backwardLastKey, paginatedData, totalCount, pageNumber_);
    //}

    //private async Task<PaginatedResponse<TProjection>> PaginateProjectionWithLastKey<TProjection>(IQueryable<TProjection> query,
    //    PaginatedRequest paginatedRequest, int pageSize, int totalCount, CancellationToken cancellationToken)
    //    where TProjection : class, IProjectionDto
    //{
    //    List<TProjection> paginatedData;
    //    int forwardLastKey = paginatedRequest.ForwardLastKey.GetValueOrDefault();
    //    int backwardLastKey = paginatedRequest.BackwardLastKey.GetValueOrDefault();

    //    if (paginatedRequest.IsBackward)
    //    {
    //        query = query.Where(x => x.SequentialId < backwardLastKey)
    //            .OrderByDescending(e => e.SequentialId);
    //        paginatedData = await query.Take(pageSize).ToListAsync(cancellationToken);
    //        (forwardLastKey, backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.First().SequentialId, paginatedData.Last().SequentialId) : (default, default);
    //        return new PaginatedResponse<TProjection>(pageSize, paginatedData, totalCount, paginatedRequest.PageNumber.GetValueOrDefault(), forwardLastKey, backwardLastKey);
    //    }

    //    query = query.Where(x => x.SequentialId > paginatedRequest.ForwardLastKey).OrderBy(e => e.SequentialId);
    //    paginatedData = await query.Take(pageSize).ToListAsync(cancellationToken);
    //    (forwardLastKey, backwardLastKey) = paginatedData.Count > 0 ? (paginatedData.Last().SequentialId, paginatedData.First().SequentialId) : (default, default);
    //    return new PaginatedResponse<TProjection>(pageSize, paginatedData, totalCount, paginatedRequest.PageNumber.GetValueOrDefault(), forwardLastKey, backwardLastKey);
    //}
}
